
using System.Collections;
using System.Collections.Generic;

using System.Text.RegularExpressions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Xml.Serialization;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics.CodeAnalysis;


/*public enum ProSym
{
    Equal,
    Bigger,
    Smaller,
    Plus,
    Times,
    LeftParentheses,
    RightParentheses,
    num,
    Atom,
    Factor,
    Term,
    Formula
}*/
public enum ProSym
{
    End,
    Equal,
    Bigger,
    Smaller,
    Plus,
    Times,
    LeftParentheses,
    RightParentheses,
    num,
    Atom,
    Factor,
    Term,
    Formula,
    Start
}


/*public enum ProSym
{
    End,
    Equal,
    Bigger,
    Smaller,
    Plus,
    Times,
    LeftParentheses,
    RightParentheses,
    num,
    Atom,
    FactorDash,
    Factor,
    TermDash,
    Term,
    FormulaDash,
    Formula,
    Start
}*/


public class LR1Item : IEqualityComparer<LR1Item>
{
    public int DestSymbol;
    public int Conversion;
    public int DotPos;
    public List<int> LASet;

    public LR1Item(int destSymbol, int conversion, int dotPos, List<int> laSet)
    {
        DotPos = dotPos;
        Conversion = conversion;
        DestSymbol = destSymbol;
        LASet = new List<int>(laSet);
    }

    public bool IsLR0PartSame(LR1Item lr1Item) 
    {
        return lr1Item.DestSymbol==DestSymbol&lr1Item.Conversion==Conversion&lr1Item.DotPos==DotPos;
    }

    public string getItemString(List<List<List<int>>> syntaxes)
    {
        string retString = $"{(ProSym)DestSymbol} => ";
        for (int i = 0; i < syntaxes[DestSymbol][Conversion].Count; i++)
        {
            if (DotPos == i)
            {
                retString += "・ ";
            }
            retString += $"{(ProSym)syntaxes[DestSymbol][Conversion][i]} ";
        }
        if (DotPos == syntaxes[DestSymbol][Conversion].Count)
        {
            retString += "・ ";
        }
        retString += "[ ";
        for (int i = 0; i < LASet.Count; i++)
        {
            retString += $"{(ProSym)LASet[i]} ";
        }
        retString += "]";
        return retString;
    }

    public static bool operator ==(LR1Item a, LR1Item b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(LR1Item a, LR1Item b)
    {
        return !a.Equals(b);
    }

    public bool Equals(LR1Item a, LR1Item b)
    {
        return a.DestSymbol == b.DestSymbol
            & a.Conversion == b.Conversion
            & a.DotPos == b.DotPos
            & a.LASet.Except(b.LASet).Count() == 0
            & b.LASet.Except(a.LASet).Count() == 0;
    }

    public int GetHashCode(LR1Item a)
    {
        string uniqueString = $"{a.DestSymbol.ToString()}/{a.Conversion.ToString()}/{a.DotPos.ToString()}";
        foreach (int i in a.LASet.Distinct().OrderBy((num) => num))
        {
            uniqueString += $"/{i}";
        }
        return uniqueString.GetHashCode();
    }

    public override int GetHashCode()
    {
        return GetHashCode(this);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is LR1Item ? Equals(this, (LR1Item)obj) : base.Equals(obj);
    }

    public List<int> GetConversion(List<List<List<int>>> syntax)
    {
        if (DestSymbol < 0 || Conversion < 0)
        {
            return null;
        }
        return syntax[DestSymbol][Conversion];
    }

}
public class ProgramTree
{
    public int TreeTypeID = 0;
    public List<ProgramTree> ProgramTrees = new List<ProgramTree>();

    public ProgramTree(int treeTypeID, List<ProgramTree> programTrees)
    {
        TreeTypeID = treeTypeID;
        ProgramTrees = programTrees;
    }
    public string TreeToStr(int depth)
    {
        string indentSpaces = "";
        for (int i = 0; i < depth; i++)
        {
            indentSpaces += "   ";
        }
        if (ProgramTrees == null || ProgramTrees.Count == 0)
        {
            return indentSpaces + $"{(ProSym)TreeTypeID} \n";
        }

        string retString = indentSpaces + $"{(ProSym)TreeTypeID}(\n";
        for (int i = 0; i < ProgramTrees.Count; i++)
        {
            retString += $"{ProgramTrees[i].TreeToStr(depth + 1)}";
        }
        retString += indentSpaces + ") \n";
        return retString;
    }
}

public class StrTokenizer
{
    List<string> TokenRegExs;
    List<string> IgnoredRegExs;

    public StrTokenizer(List<string> tokenRegExs, List<string> ignoredRegExs)
    {
        TokenRegExs = tokenRegExs;
        IgnoredRegExs = ignoredRegExs;
    }

    public List<ProgramTree> TokenizeString(string sourceCode)
    {

        List<ProgramTree> teriminalSymbols = new List<ProgramTree>();
        string restSourceCode = sourceCode;
        while (restSourceCode.Length > 0)
        {
            bool isRegExMatched = false;
            for (int i = 0; i < TokenRegExs.Count; i++)
            {
                Match match = Regex.Match(restSourceCode, TokenRegExs[i]);
                if (match.Success && match.Index == 0 && match.Length > 0)
                {
                    Console.WriteLine($"Matched:{match.Value}");
                    Console.WriteLine((ProSym)i);
                    teriminalSymbols.Add(new ProgramTree(i, null));
                    restSourceCode = restSourceCode.Substring(match.Length);
                    isRegExMatched = true;
                    break;
                }
            }
            if (isRegExMatched)
            {
                continue;
            }

            for (int i = 0; i < IgnoredRegExs.Count; i++)
            {
                Match match = Regex.Match(restSourceCode, IgnoredRegExs[i]);
                if (match.Success && match.Index == 0 && match.Length > 0)
                {
                    Console.WriteLine($"Ignored:{match.Value}");
                    restSourceCode = restSourceCode.Substring(match.Length);
                    isRegExMatched = true;
                    break;
                }
            }

            if (isRegExMatched)
            {
                continue;
            }

            Console.WriteLine($"Unexpected:{restSourceCode[0]}");
            restSourceCode = restSourceCode.Substring(1);
        }

        return teriminalSymbols;
    }
}



public class LALR1Analyzer
{
    public int Smallest_NonterminalSymbol;
    public List<List<List<int>>> Syntaxes;
    public List<AutomatonState> ExistingAutomatonStates;


    public ProgramTree analyze(List<ProgramTree> terminalSymbols)
    {
        List<AutomatonState> StateTrail = new List<AutomatonState>();
        StateTrail.Add(ExistingAutomatonStates[0]);//現在のステートはStateTrailの最後尾( StateTrail[StateTrail.Count-1])
        List<ProgramTree> PushdTree = new List<ProgramTree>();
        int curTerminalSymbolIndex = 0;
        while (curTerminalSymbolIndex < terminalSymbols.Count)//終了記号が読み込まれたら(=終了記号以前の終端記号列が完全に還元されたことと同値)
        {
            Console.WriteLine($"State:{ExistingAutomatonStates.IndexOf(StateTrail[StateTrail.Count - 1])}");
            string RestTerminalSymbolStr = " RestTerminalSymbol:";
            for (int i = curTerminalSymbolIndex; i < terminalSymbols.Count; i++)
            {
                RestTerminalSymbolStr += $"{(ProSym)terminalSymbols[i].TreeTypeID} ";
            }
            Console.WriteLine(RestTerminalSymbolStr);
            string PushedTreeStr = " PushedTree:";
            for (int i = 0; i < PushdTree.Count; i++)
            {
                PushedTreeStr += $"{(ProSym)PushdTree[i].TreeTypeID} ";
            }
            Console.WriteLine(PushedTreeStr);
            Console.WriteLine("");



            ProgramTree nextPushedSymbol = terminalSymbols[curTerminalSymbolIndex];
            LR1Item nextReduction = StateTrail[StateTrail.Count - 1].Reductions[nextPushedSymbol.TreeTypeID];
            if (nextReduction.DotPos >= 0)// DotPosが0以上(還元操作を行うことを意味する)なら、還元を行う
            {
                int reducedTo = nextReduction.DestSymbol;
                List<int> reductionConversion = Syntaxes[nextReduction.DestSymbol][nextReduction.Conversion];
                List<ProgramTree> reducedSymbols = new List<ProgramTree>(PushdTree.GetRange(PushdTree.Count - reductionConversion.Count, reductionConversion.Count));
                PushdTree.RemoveRange(PushdTree.Count - reductionConversion.Count, reductionConversion.Count);
                nextPushedSymbol = new ProgramTree(reducedTo, reducedSymbols);

                curTerminalSymbolIndex--;//還元する場合、終端記号を消費しないので、後ろで行われるインクリメントを打ち消す為に、ここでデクリメントしておく。

                StateTrail.RemoveRange(StateTrail.Count - reductionConversion.Count, reductionConversion.Count);

            }


            AutomatonState NextShiftState = StateTrail[StateTrail.Count - 1].Transitions[nextPushedSymbol.TreeTypeID];
            if (NextShiftState == null)
            {
                Console.WriteLine("ERROR!");
                break;
            }
            StateTrail.Add(NextShiftState);
            curTerminalSymbolIndex++;
            PushdTree.Add(nextPushedSymbol);
        }
        return PushdTree[0];
    }

    public LALR1Analyzer(int smallest_NonterminalSymbol, List<List<List<int>>> syntaxes)
    {
        Smallest_NonterminalSymbol = smallest_NonterminalSymbol;
        Syntaxes = syntaxes;
        MakeAutomaton();
    }

    public void MakeAutomaton()
    {
        Console.WriteLine("MakeAutomaton----------");
        List<LR1Item> startStateInitItems = new List<LR1Item>();
        startStateInitItems.Add(new LR1Item(Syntaxes.Count - 1, 0, 0, new List<int>()));
        AutomatonState StartState = new AutomatonState(startStateInitItems, Smallest_NonterminalSymbol, Syntaxes);
        ExistingAutomatonStates = new List<AutomatonState>();
        ExistingAutomatonStates.Add(StartState);
        for (int i = 0; i < ExistingAutomatonStates.Count; i++)
        {
            
            //Console.WriteLine($"State{i}");
            //Console.WriteLine("LR1Item:");
            for (int v = 0; v < ExistingAutomatonStates[i].LR1Items.Count; v++)
            {
                //Console.WriteLine(ExistingAutomatonStates[i].LR1Items[v].getItemString(Syntaxes));
            }
            //Console.WriteLine($"Transition:");

            for (int aSymbol = 0; aSymbol < Syntaxes.Count; aSymbol++) //ExistingAutomatonStates[i]にいるときに各記号をプッシュされたときの
            {
                AutomatonState newState = ExistingAutomatonStates[i].GetNextState(aSymbol); //遷移先のステートを作成


                if (newState == null)
                {
                    continue;
                }
                List<AutomatonState> ExsistingStates = ExistingAutomatonStates.Where(o => o.IsLR0PartsSame(newState)).ToList();
                if (ExsistingStates.Count() != 0)//すでに同じLR0アイテム(LR1ののLASetを除いた部分という意味合い)集合を持つステートがある場合
                {
                    ExsistingStates[0].UpdateLASet(newState);
                    ExistingAutomatonStates[i].Transitions[aSymbol] = ExsistingStates[0];
                    //Console.WriteLine($"{i}-({(ProSym)aSymbol})->{ExistingAutomatonStates.IndexOf(ExsistingStates[0])}");
                    continue;
                }
                else
                {
                    ExistingAutomatonStates.Add(newState);//ExistingAutomatonStatesに格納する
                    ExistingAutomatonStates[i].Transitions[aSymbol] = newState;
                    //Console.WriteLine($"{i}-({(ProSym)aSymbol})->{ExistingAutomatonStates.Count-1}");

                }
            }

        }
        for (int i = 0; i < ExistingAutomatonStates.Count; i++) 
        {
            ExistingAutomatonStates[i].MakeReductions();
        }

        for (int i = 0; i < ExistingAutomatonStates.Count; i++) 
        {
            Console.WriteLine($"State{i}");
            Console.WriteLine("LR1Item:");
            for (int v = 0; v < ExistingAutomatonStates[i].LR1Items.Count; v++)
            {
                Console.WriteLine(ExistingAutomatonStates[i].LR1Items[v].getItemString(Syntaxes));
            }
            Console.WriteLine("");
            Console.WriteLine("構文解析表:");
            for (int v = 0; v < ExistingAutomatonStates[i].Reductions.Count; v++)
            {
                if (ExistingAutomatonStates[i].Reductions[v].DotPos >= 0)
                {
                    Console.WriteLine($"{(ProSym)v}   Reduction   {ExistingAutomatonStates[i].Reductions[v].getItemString(Syntaxes)}");
                }
                else if (ExistingAutomatonStates[i].Transitions[v] != null)
                {
                    Console.WriteLine($"{(ProSym)v}   ShiftTo   {ExistingAutomatonStates.IndexOf(ExistingAutomatonStates[i].Transitions[v])}");
                }
                else
                {
                    Console.WriteLine($"{(ProSym)v}   Error");
                }
            }

            Console.WriteLine("\n");
        }    
        Console.WriteLine("MakeAutomaton----------(Fin)");
    }


    public class AutomatonState
    {
        public List<LR1Item> LR1Items;
        public List<AutomatonState> Transitions;
        public List<LR1Item> Reductions;


        public int Smallest_NonterminalSymbol;
        public List<List<List<int>>> Syntaxes;

        public AutomatonState(List<LR1Item> initLR1Items, int smallest_NonterminalSymbol, List<List<List<int>>> syntaxes)
        {
            Smallest_NonterminalSymbol = smallest_NonterminalSymbol;
            Syntaxes = syntaxes;
            Transitions = new List<AutomatonState>();
            for (int i = 0; i < syntaxes.Count; i++)
            {
                Transitions.Add(null);
            }
            
            MakeLR1Items(initLR1Items);


        }

        public bool IsItemsSame(AutomatonState state)
        {
            return LR1Items.Except(state.LR1Items).Count() == 0 && state.LR1Items.Except(LR1Items).Count() == 0;
        }

        public bool IsLR0PartsSame(AutomatonState state) 
        {
            foreach (LR1Item aItem in state.LR1Items) 
            {
                if (LR1Items.Where((o) => o.IsLR0PartSame(aItem)).Count() == 0) 
                {
                    return false;
                }
            }
            foreach (LR1Item aItem in LR1Items)
            {
                if (state.LR1Items.Where((o) => o.IsLR0PartSame(aItem)).Count() == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void UpdateLASet(AutomatonState IntegratedState) 
        {
            foreach (LR1Item aItem in IntegratedState.LR1Items)
            {
                LR1Item LR0PartsSameItem= LR1Items.Where((o) => o.IsLR0PartSame(aItem)).ToList()[0];
                LR0PartsSameItem.LASet.AddRange(aItem.LASet);
                LR0PartsSameItem.LASet = LR0PartsSameItem.LASet.Distinct().ToList();
            }
        }

        private void MakeLR1Items(List<LR1Item> InitItems)
        {
            LR1Items = new List<LR1Item>(InitItems);

            while (true)
            {
                List<LR1Item> NextLR1Items = new List<LR1Item>(LR1Items);
                foreach (LR1Item aItem in LR1Items)//LR1Itemsの各要素について
                {

                    if (Syntaxes[aItem.DestSymbol][aItem.Conversion].Count <= aItem.DotPos) //・の次の要素がない場合、aItemからLR1Itemsは増えないので、次に行く
                    {
                        continue;
                    }
                    int dotNextSymbol = Syntaxes[aItem.DestSymbol][aItem.Conversion][aItem.DotPos];//・の次の記号
                    int dotNextNextSymbol = Syntaxes[aItem.DestSymbol][aItem.Conversion].Count <= (aItem.DotPos + 1) ? -1 : Syntaxes[aItem.DestSymbol][aItem.Conversion][aItem.DotPos + 1];//dotの次の次の記号(次の次に記号がない場合の値は-1)
                    if (dotNextSymbol >= Smallest_NonterminalSymbol)//・の次の記号が非終端記号なら
                    {
                        List<int> NextLRItemsLASet = aItem.LASet;
                        if (dotNextNextSymbol >= 0)
                        {
                            NextLRItemsLASet = GetFIrst(dotNextNextSymbol, new List<int>());

                        }
                        for (int conversion = 0; conversion < Syntaxes[dotNextSymbol].Count; conversion++)//・の次の記号への変換それぞれについて
                        {
                            LR1Item NewItem = new LR1Item(dotNextSymbol, conversion, 0, NextLRItemsLASet);//その変換の読み始め(X=>・Y)のアイテムが
                            if (!NextLR1Items.Contains(NewItem))//まだNextLR1Itemsに存在していなければNextLR1Itemsに追加する
                            {
                                List<LR1Item> LR1Item_SameLR0Part = NextLR1Items.Where((o) => o.DestSymbol == NewItem.DestSymbol & o.Conversion == NewItem.Conversion & o.DotPos == NewItem.DotPos).ToList();
                                if (LR1Item_SameLR0Part.Count > 0)
                                {
                                    LR1Item_SameLR0Part[0].LASet.AddRange(NewItem.LASet);
                                    LR1Item_SameLR0Part[0].LASet = LR1Item_SameLR0Part[0].LASet.Distinct().ToList();
                                }
                                else
                                {
                                    NextLR1Items.Add(NewItem);
                                }
                                //NextLR1Items.Add(NewItem);
                            }
                        }
                    }
                }

                NextLR1Items = NextLR1Items.Distinct().ToList();
                (NextLR1Items, LR1Items) = (LR1Items, NextLR1Items);
                if (LR1Items.Except(NextLR1Items).Count() <= 0)// LR1ItemsからNextLR1Itemsにかけて変化がなかったら終了
                {
                    break;
                }
            }
        }

        public void MakeReductions()
        {
            Reductions = new List<LR1Item>();
            for (int i = 0; i < Syntaxes.Count; i++)
            {
                Reductions.Add(new LR1Item(-1, -1, -1, new List<int>()));
            }
            for (int i = 0; i < LR1Items.Count; i++)//各LR1temについて
            {
                if (Syntaxes[LR1Items[i].DestSymbol][LR1Items[i].Conversion].Count() <= LR1Items[i].DotPos) //Dotが最後にある(還元可能な)アイテムならば
                {

                    for (int v = 0; v < Reductions.Count; v++)
                    {
                        if (LR1Items[i].LASet.Contains(v) || LR1Items[i].LASet.Count == 0)
                        {
                            if (Reductions[v].DotPos>=0) 
                            {
                                Console.WriteLine("還元-還元競合");
                            }
                            if (Transitions[v]!=null) 
                            {
                                Console.WriteLine("シフト-還元競合");
                            }
                            Reductions[v] = LR1Items[i];//そのアイテムへの還元を行う
                        }
                    }
                }
            }
        }

        public AutomatonState GetNextState(int pushedSymbol)
        {
            List<LR1Item> NextStateInitLR1Items = new List<LR1Item>();
            foreach (LR1Item aItem in LR1Items) //各aItemのdotの次の記号がpushedSymbolであるなら、aItemのdotを次に進めたitemをTrasitStateLR1Itemsに追加
            {
                if (Syntaxes[aItem.DestSymbol][aItem.Conversion].Count > aItem.DotPos && Syntaxes[aItem.DestSymbol][aItem.Conversion][aItem.DotPos] == pushedSymbol)
                {
                    NextStateInitLR1Items.Add(new LR1Item(aItem.DestSymbol, aItem.Conversion, aItem.DotPos + 1, aItem.LASet));
                }
            }
            if (NextStateInitLR1Items.Count > 0)
            {
                return new AutomatonState(NextStateInitLR1Items, Smallest_NonterminalSymbol, Syntaxes);
            }
            else
            {
                return null;
            }

        }

        private List<int> GetFIrst(int symbol, List<int> stacks)
        {
            List<int> result = new List<int>();
            if (stacks.Contains(symbol))//LR1文法は左再帰があるので、無限ループを起こさないように、今までの再帰呼び出しでsymbolに渡された値を確認している
            {
                return result;
            }
            else if (symbol < Smallest_NonterminalSymbol)//symbolが終端記号の場合
            {
                result.Add(symbol);
            }
            else //Symbolが非終端記号の場合
            {
                foreach (List<int> oneOfSyntax in Syntaxes[symbol])
                {
                    if (oneOfSyntax.Count > 0)//
                    {
                        stacks.Add(symbol);
                        result.AddRange(GetFIrst(oneOfSyntax[0], stacks));
                        stacks.RemoveAt(stacks.Count - 1);
                    }
                    else//空文字列に変換できる場合(LR1文法では空文字列に変換できる終端記号はないので、基本ここは通らない)
                    {
                        result.Add(-1);//空文字を表す-1を代入
                    }
                }
            }
            return result;
        }
    }
}



