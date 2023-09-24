// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

List<string> regExs = new List<string>(new string[] {
            "",
            "==",
            ">",
            "<",
            @"\+",
            @"\*",
            @"\(",
            @"\)",
            "[0-9]+"
        });
List<string> ignoredRegExs = new List<string>(new string[] {
            "(//.*(\r\n|\n|\r)|\\s+)"
        }); ;

string sourceCode = "21+31+(29+40)*10+20*30";

List<ProgramTree> terminalSymbols = new StrTokenizer(regExs, ignoredRegExs).TokenizeString(sourceCode);
terminalSymbols.Add(new ProgramTree((int)ProSym.End, new List<ProgramTree>()));

/*List<List<int>> atomSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.num}),
            new List<int>(new int[]{(int)ProSym.LeftParentheses,(int)ProSym.Formula,(int)ProSym.RightParentheses})
        });
List<List<int>> factorSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.Atom,(int)ProSym.FactorDash})
        });
List<List<int>> factorDashSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.Times,(int)ProSym.Factor}),
            new List<int>()
        });
List<List<int>> termSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.Factor,(int)ProSym.TermDash})
        });
List<List<int>> termDashSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.Plus,(int)ProSym.Term}),
            new List<int>()
        });
List<List<int>> formulaSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.Term,(int)ProSym.FormulaDash })
        });
List<List<int>> formulaDashSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.Bigger,(int)ProSym.Formula}),
            new List<int>(new int[]{(int)ProSym.Equal,(int)ProSym.Formula}),
            new List<int>()
        });
List<List<int>> startSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.Formula,(int)ProSym.End })
        });*/

List<List<int>> atomSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.num}),
            new List<int>(new int[]{(int)ProSym.LeftParentheses,(int)ProSym.Formula,(int)ProSym.RightParentheses})
        });
List<List<int>> factorSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.Factor,(int)ProSym.Times,(int)ProSym.Atom}),
            new List<int>(new int[]{(int)ProSym.Atom})
        });

List<List<int>> termSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.Term,(int)ProSym.Plus,(int)ProSym.Factor}),
            new List<int>(new int[]{(int)ProSym.Factor})
        });
List<List<int>> formulaSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.Formula,(int)ProSym.Bigger,(int)ProSym.Term }),
            new List<int>(new int[]{(int)ProSym.Formula,(int)ProSym.Equal,(int)ProSym.Term }),
            new List<int>(new int[]{(int)ProSym.Term})
        });
List<List<int>> startSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym.Formula,(int)ProSym.End })
        });



List<List<List<int>>> syntax = new List<List<List<int>>>((int)ProSym.Start + 1);

for (int i = 0; i <= (int)ProSym.Start; i++)
{
    syntax.Add(new List<List<int>>());
}
Console.WriteLine(syntax.Count);
Console.WriteLine((int)ProSym.Atom);
syntax[(int)ProSym.Atom] = atomSyntax;
syntax[(int)ProSym.Factor] = factorSyntax;
syntax[(int)ProSym.Term] = termSyntax;
syntax[(int)ProSym.Formula] = formulaSyntax;
syntax[(int)ProSym.Start] = startSyntax;
/*syntax[(int)ProSym.Atom] = atomSyntax;
syntax[(int)ProSym.Factor] = factorSyntax;
syntax[(int)ProSym.FactorDash] = factorDashSyntax;
syntax[(int)ProSym.Term] = termSyntax;
syntax[(int)ProSym.TermDash] = termDashSyntax;
syntax[(int)ProSym.Formula] = formulaSyntax;
syntax[(int)ProSym.FormulaDash] = formulaDashSyntax;
syntax[(int)ProSym.Start] = startSyntax;*/


LALR1Analyzer lr0Analyzer = new LALR1Analyzer((int)ProSym.Atom, syntax);


Console.WriteLine(lr0Analyzer.analyze(terminalSymbols).TreeToStr(0));

LR1Item a = new LR1Item(1, 1, 1, new List<int>(new int[] { 1, 2 }));
LR1Item b = new LR1Item(1, 1, 1, new List<int>(new int[] { 2, 1 }));
LR1Item c = new LR1Item(1, 1, 1, new List<int>(new int[] { 1, 2, 3 }));
LR1Item d = new LR1Item(1, 1, 1, new List<int>(new int[] { 1, 4, 3 }));
LR1Item e = new LR1Item(1, 1, 1, new List<int>(new int[] { 1, 3, 4 }));
List<LR1Item> list_a = new List<LR1Item>(new LR1Item[] { a, b, c, d, e, a });
Console.WriteLine(list_a.Distinct().Count());

Console.WriteLine(list_a.Contains(new LR1Item(1, 1, 1, new List<int>(new int[] { 1, 3, 4 }))));

Console.WriteLine(a.Equals(d, e));

Console.WriteLine($"{e == d}");

