// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

/*
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

List<ProgramTree> terminalSymbols = new StrTokenizer(regExs, ignoredRegExs, Enum.GetNames(typeof(ProSym)).ToList()).TokenizeString(sourceCode);
terminalSymbols.Add(new ProgramTree((int)ProSym.End, new List<ProgramTree>()));

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


syntax[(int)ProSym.Atom] = atomSyntax;
syntax[(int)ProSym.Factor] = factorSyntax;
syntax[(int)ProSym.Term] = termSyntax;
syntax[(int)ProSym.Formula] = formulaSyntax;
syntax[(int)ProSym.Start] = startSyntax;

LALR1Analyzer lr0Analyzer = new LALR1Analyzer((int)ProSym.Atom, syntax, Enum.GetNames(typeof(ProSym)).ToList());
Console.WriteLine(lr0Analyzer.analyze(terminalSymbols).TreeToStr(0,Enum.GetNames(typeof(ProSym)).ToList()));



foreach (string s in Enum.GetNames(typeof(ProSym2))) 
{
    Console.WriteLine(s);
}
*/

List<string> regExs2 = new List<string>(new string[] {
            "",
            "==",
            ">",
            "<",
            @"\+",
            @"\*",
            @"\(",
            @"\)",
            @"\{",
            @"\}",
            "[0-9]+",
            "=",
            ";",
            "if",
            "else",
            "while",
            "[_a-zA-Z][[_a-zA-Z0-9]*",
        });
List<string> ignoredRegExs2 = new List<string>(new string[] {
            "(//.*(\r\n|\n|\r)|\\s+)"
        }); ;

string sourceCode2 =
    "pere=21+31+(29+40)*10+20*30;" +
    "if(pere==1352)" +
    "{" +
    "rew=pere*2;" +
    "pere=rew+2;" +
    "}" +
    "else" +
    "{" +
    "rew=pere*3;" +
    "pere=rew+3;" +
    "}";

List<ProgramTree> terminalSymbols2 = new StrTokenizer(regExs2, ignoredRegExs2, Enum.GetNames(typeof(ProSym2)).ToList()).TokenizeString(sourceCode2);
terminalSymbols2.Add(new ProgramTree((int)ProSym2.End, new List<ProgramTree>()));

List<List<int>> atom2Syntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym2.num}),
            new List<int>(new int[]{(int)ProSym2.Identifier}),
            new List<int>(new int[]{(int)ProSym2.LeftParentheses,(int)ProSym2.Formula,(int)ProSym2.RightParentheses})
        });
List<List<int>> factor2Syntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym2.Factor,(int)ProSym2.Times,(int)ProSym2.Atom}),
            new List<int>(new int[]{(int)ProSym2.Atom})
        });

List<List<int>> term2Syntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym2.Term,(int)ProSym2.Plus,(int)ProSym2.Factor}),
            new List<int>(new int[]{(int)ProSym2.Factor})
        });
List<List<int>> formula2Syntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym2.Formula,(int)ProSym2.Bigger,(int)ProSym2.Term }),
            new List<int>(new int[]{(int)ProSym2.Formula,(int)ProSym2.Equal,(int)ProSym2.Term }),
            new List<int>(new int[]{(int)ProSym2.Term})
        });
List<List<int>> start2Syntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym2.Statements,(int)ProSym2.End })
        });
List<List<int>> ifStatementSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym2.IfKeyword,(int)ProSym2.LeftParentheses,(int)ProSym2.Formula,(int)ProSym2.RightParentheses,(int)ProSym2.LeftBracket,(int)ProSym2.Statements,(int)ProSym2.RightBracket}),
            new List<int>(new int[]{(int)ProSym2.IfKeyword,(int)ProSym2.LeftParentheses,(int)ProSym2.Formula,(int)ProSym2.RightParentheses,(int)ProSym2.LeftBracket,(int)ProSym2.Statements,(int)ProSym2.RightBracket,(int)ProSym2.ElseKeyword,(int)ProSym2.LeftBracket,(int)ProSym2.Statements,(int)ProSym2.RightBracket})
        });
List<List<int>> whileStatementSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym2.WhileKeyword,(int)ProSym2.LeftParentheses,(int)ProSym2.Formula,(int)ProSym2.RightParentheses,(int)ProSym2.LeftBracket,(int)ProSym2.Statements,(int)ProSym2.RightBracket})
        });

List<List<int>> statementSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym2.Identifier,(int)ProSym2.Assign,(int)ProSym2.Formula,(int)ProSym2.Semicolon}),
            new List<int>(new int[]{(int)ProSym2.WhileStatement}),
            new List<int>(new int[]{(int)ProSym2.IfStatement}),
});

List<List<int>> statementsSyntax = new List<List<int>>(new List<int>[] {
            new List<int>(new int[]{(int)ProSym2.Statements,(int)ProSym2.Statement}),
            new List<int>(new int[]{(int)ProSym2.Statement})
});

List<List<List<int>>> syntax2 = new List<List<List<int>>>((int)ProSym2.Start + 1);

for (int i = 0; i <= (int)ProSym2.Start; i++)
{
    syntax2.Add(new List<List<int>>());
}

syntax2[(int)ProSym2.Atom] = atom2Syntax;
syntax2[(int)ProSym2.Factor] = factor2Syntax;
syntax2[(int)ProSym2.Term] = term2Syntax;
syntax2[(int)ProSym2.Formula] = formula2Syntax;
syntax2[(int)ProSym2.IfStatement] = ifStatementSyntax;
syntax2[(int)ProSym2.WhileStatement] = whileStatementSyntax;
syntax2[(int)ProSym2.Statement] = statementSyntax;
syntax2[(int)ProSym2.Statements] = statementsSyntax;
syntax2[(int)ProSym2.Start] = start2Syntax;

LALR1Analyzer lr0Analyzer2 = new LALR1Analyzer((int)ProSym2.Atom, syntax2, Enum.GetNames(typeof(ProSym2)).ToList());
Console.WriteLine(lr0Analyzer2.analyze(terminalSymbols2).TreeToStr(0, Enum.GetNames(typeof(ProSym2)).ToList()));

