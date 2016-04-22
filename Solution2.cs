using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LittleAssembler
{

    public enum Register
    {
        A,
        B,
        C,
        D

    }

    public class AssemblerImitator
    {
        private const string FREE_REG = "";

        /// <summary>
        /// Память
        /// </summary>
        public List<string> Memory = new List<string>();

        /// <summary>
        /// Регистры
        /// </summary>
        public Dictionary<Register, string> Registers = new Dictionary<Register, string>();

        /// <summary>
        /// Команды 
        /// </summary>
        public List<String> commands = new List<String>();


        public AssemblerImitator()
        {
            foreach (Register reg in Enum.GetValues(typeof(Register)))
            {
                Registers.Add(reg, FREE_REG); 
            }

            Memory.Add("solution");
        }

        public void MOVMemToReg(string memoryValue, Register reg)
        {
            int i = Memory.FindIndex(x => x == memoryValue);
            Registers[reg] = Memory[i];
            commands.Add(String.Format("MOV {0} {1}", reg, i.ToString()));

        }

        public void MOVRegToNewMem(string data, Register reg)
        {
            Memory.Add(data);
            Registers[reg] = Memory.Last();
            commands.Add(String.Format("MOV {0} {1}", Memory.Count - 1, reg));

        }

        public void MUL(Register reg1, Register reg2)
        {
            commands.Add(String.Format("MUL {0} {1}", reg1, reg2));

        }

        public void ADD(Register reg1, Register reg2)
        {
            commands.Add(String.Format("ADD {0} {1}", reg1, reg2));

        }

        public void SUB(Register reg1, Register reg2)
        {
            commands.Add(String.Format("SUB {0} {1}", reg1, reg2));

        }

        public void DIV(Register reg1, Register reg2)
        {
            commands.Add(String.Format("DIV {0} {1}", reg1, reg2));

        }

        public void Resolve(Register reg)
        {
            Memory[0] = Registers[reg];
            commands.Add(String.Format("MOV 0 {0}", reg));

        }

    }

    public class Translator
    {

        private static AssemblerImitator imitator;

        /// <summary>
        /// Разобранное дерево выражений
        /// </summary>
        private static List<KeyValuePair<ExpressionType, String>> parsedTree = new List<KeyValuePair<ExpressionType, String>>();
        
        private static int argumentCount = 0;


        #region перегрузки метода Translate
        public static List<String> Translate(Expression<Func<int, int, int>> exprTree)
        {
            BinaryExpression binExpr = (BinaryExpression)exprTree.Body;
            return TranslateBinary(binExpr);

        }

        public static List<String> Translate(Expression<Func<int, int, int, int>> exprTree)
        {           
            BinaryExpression binExpr = (BinaryExpression)exprTree.Body;
            return TranslateBinary(binExpr);

        }

        public static List<String> Translate(Expression<Func<int, int, int, int, int>> exprTree)
        {
            BinaryExpression binExpr = (BinaryExpression)exprTree.Body;
            return TranslateBinary(binExpr);

        }

        public static List<String> Translate(Expression<Func<int, int, int, int, int, int>> exprTree)
        {
            BinaryExpression binExpr = (BinaryExpression)exprTree.Body;
            return TranslateBinary(binExpr);

        }

        public static List<String> Translate(Expression<Func<int, int, int, int, int, int, int>> exprTree)
        {
            BinaryExpression binExpr = (BinaryExpression)exprTree.Body;
            return TranslateBinary(binExpr);

        }

        public static List<String> Translate(Expression<Func<int, int, int, int, int, int, int, int>> exprTree)
        {
            BinaryExpression binExpr = (BinaryExpression)exprTree.Body;
            return TranslateBinary(binExpr);

        }

        public static List<String> Translate(Expression<Func<int, int, int, int, int, int, int, int, int>> exprTree)
        {
            BinaryExpression binExpr = (BinaryExpression)exprTree.Body;
            return TranslateBinary(binExpr);

        }

        public static List<String> Translate(Expression<Func<int, int, int, int, int, int, int, int, int, int>> exprTree)
        {
            BinaryExpression binExpr = (BinaryExpression)exprTree.Body;
            return TranslateBinary(binExpr);

        }

        public static List<String> Translate(Expression<Func<int, int, int, int, int, int, int, int, int, int, int>> exprTree)
        {
            BinaryExpression binExpr = (BinaryExpression)exprTree.Body;
            return TranslateBinary(binExpr);

        }
        #endregion

        /// <summary>
        /// Разбор дерева выражений
        /// </summary>
        public static List<String> TranslateBinary(BinaryExpression binExpr)
        {
            Expression left = binExpr.Left;
            Expression right = binExpr.Right;

            ParseLeaf(left);
            AddSymbol(binExpr);
            ParseLeaf(right);

            //инициализация имитации ассемблера
            imitator = new AssemblerImitator();

            FillMemory();

            ExpressionAnalyze();

            return imitator.commands;

        }

        /// <summary>
        /// заполнение памяти псевдоассемблера начальными значениями 
        /// </summary>
        private static void FillMemory()
        {
            foreach (var item in parsedTree)
            {
                //если следующий элемент выражения является константой или параметром, сохранить его в память
                if (item.Key == ExpressionType.Constant || item.Key == ExpressionType.Parameter)
                {
                    if (!imitator.Memory.Contains(item.Value))
                    {
                        imitator.Memory.Add(item.Value);
                    }
                }
            }

        }

        /// <summary>
        /// Последовательный перебор каждого действия в выражении,  и составление ассемблерных команд
        /// </summary>
        private static void ExpressionAnalyze()
        {
            do
            {
                int counter = 0;
                bool rewrite = true;
                string operation = "";
                string argument1 = "";
                string argument2 = "";
                //первая итерация проверки выражения      
                foreach (var item in parsedTree)
                {
                    //если текущий элемент выражения является оператором
                    if (item.Key == ExpressionType.Multiply || item.Key == ExpressionType.Divide || item.Key == ExpressionType.Subtract || item.Key == ExpressionType.Add)
                    {
                        operation = item.Value;

                        //если следующие два элемента выражения не существуют, прекратить цикл
                        if (counter + 2 >= parsedTree.Count)
                        {
                            rewrite = false;
                            break;
                        }

                        //если следующие два элемента выражения являются константами или параметрами
                        var arg1 = parsedTree[counter + 1];
                        var arg2 = parsedTree[counter + 2];

                        argument1 = arg1.Value;
                        argument2 = arg2.Value;

                        if ((arg1.Key == ExpressionType.Constant || arg1.Key == ExpressionType.Parameter) && (arg2.Key == ExpressionType.Constant || arg2.Key == ExpressionType.Parameter))
                        {
                            imitator.MOVMemToReg(arg1.Value.ToString(), Register.A);
                            imitator.MOVMemToReg(arg2.Value.ToString(), Register.B);
                            WriteAssemblerOperation(item);
                            //заменить эти три элемента одной константой
                            rewrite = true;
                            break;
                        }
                    }

                    counter++;
                }

                if (rewrite)
                {
                    RewriteTree(counter, operation, argument1, argument2);
                    continue;
                }

                int counter2 = 0;
                bool rewrite2 = true;
                //вторая итерация проверки выражения
                foreach (var item in parsedTree)
                {
                    //если текущий элемент выражения является константой, или параметром
                    if (item.Key == ExpressionType.Constant || item.Key == ExpressionType.Parameter)
                    {
                        operation = item.Value;

                        //если следующие два элемента выражения не существуют, прекратить цикл
                        if (counter2 + 2 >= parsedTree.Count)
                        {
                            rewrite = false;
                            break;
                        }

                        var arg1 = parsedTree[counter];
                        var arg2 = parsedTree[counter + 1];

                        argument2 = arg1.Value + arg2.Value;

                        //если следующий элемент выражения является оператором
                        if (arg1.Key == ExpressionType.Multiply || arg1.Key == ExpressionType.Divide || arg1.Key == ExpressionType.Subtract || arg1.Key == ExpressionType.Add)
                        {
                            //а следующий после оператора является константой, или параметром
                            if (arg2.Key == ExpressionType.Constant || arg2.Key == ExpressionType.Parameter)
                            {
                                imitator.MOVMemToReg(item.Value.ToString(), Register.A);
                                imitator.MOVMemToReg(arg2.Value.ToString(), Register.B);
                                WriteAssemblerOperation(arg1);
                                //заменить эти три элемента одной константой
                                rewrite2 = true;
                                break;
                            }
                        }
                    }

                    counter2++;
                }

                if (rewrite2)
                {
                    RewriteTree(counter2, operation, argument1, argument2);

                    if (parsedTree.Count == 1)
                    {
                        imitator.Resolve(Register.A);
                    }

                    continue;
                }

            } while (parsedTree.Count > 1);

        }

        /// <summary>
        /// Переписать действие в дереве выражения, заменив его новой константой
        /// </summary>
        private static void RewriteTree(int rewriteCounter, string operation, string arg1, string arg2)
        {
            List<KeyValuePair<ExpressionType, String>> newParsedTree = new List<KeyValuePair<ExpressionType, String>>();

            for (int i = 0; i < parsedTree.Count; i++)
            {
                var oldElement = parsedTree[i];

                if (i == rewriteCounter + 1 || i == rewriteCounter + 2)
                {
                    continue;
                }

                if (i == rewriteCounter)
                {
                    int intArg1 = 0;
                    int intArg2 = 0;
                    int.TryParse(arg1, out intArg1);
                    int.TryParse(arg2, out intArg2);
                    int result;
                    string newArgName = "";

                    if (intArg1 != 0 && intArg2 != 0)
                    {
                        result = DoOperation(intArg1, intArg2, ((ExpressionType)Enum.Parse(typeof(ExpressionType), operation)));
                        newArgName = result.ToString();
                    }
                    else
                    {
                        newArgName = arg1 + operation + arg2;
                    }
                    
                    newParsedTree.Add(new KeyValuePair<ExpressionType, string>(ExpressionType.Constant, newArgName));

                    //записать команды ассемблера
                    imitator.MOVRegToNewMem(newArgName, Register.A);
                }
                else
                {
                    newParsedTree.Add(oldElement);
                }

            }

            parsedTree = newParsedTree;
            
        }

        private static int DoOperation(int arg1, int arg2, ExpressionType operator_)
        {
            int res = 0;

            if (operator_ == ExpressionType.Add)
            {
                res = arg1 + arg2;
            }
            else
            if (operator_ == ExpressionType.Multiply)
            {
                res = arg1 * arg2;
            }
            else
            if (operator_ == ExpressionType.Divide)
            {
                res = arg1 / arg2;
            }
            else
            if (operator_ == ExpressionType.Subtract)
            {
                res = arg1 - arg2;
            }

            return res;
        }

        private static void WriteAssemblerOperation(KeyValuePair<ExpressionType, String> operator_)
        {
            if (operator_.Key == ExpressionType.Add)
            {
                imitator.ADD(Register.A, Register.B);
            }
            else
            if (operator_.Key == ExpressionType.Multiply)
            {
                imitator.MUL(Register.A, Register.B);
            }
            else
            if (operator_.Key == ExpressionType.Divide)
            {
                imitator.DIV(Register.A, Register.B);
            }
            else
            if (operator_.Key == ExpressionType.Subtract)
            {
                imitator.SUB(Register.A, Register.B);
            }

        }

        /// <summary>
        /// Синтаксический разбор части дерева выражений
        /// </summary>
        private static void ParseLeaf(Expression partOfTree)
        {
            bool nodeIsParameter = partOfTree.NodeType == ExpressionType.Parameter;
            bool nodeIsConstant = partOfTree.NodeType == ExpressionType.Constant;
            
            if (!nodeIsParameter && !nodeIsConstant)
            {
                AddSymbol(partOfTree);

                ParseLeaf(((BinaryExpression)partOfTree).Left);
                ParseLeaf(((BinaryExpression)partOfTree).Right);
            }
            else
            {
                argumentCount++;

                AddSymbol(partOfTree);
                
            }

            return;
        }

        /// <summary>
        /// Добавление в список элементов выражения параметр, либо оператор
        /// </summary>
        private static void AddSymbol(Expression exp)
        {

            if (exp.NodeType == ExpressionType.Constant)
            {
                parsedTree.Add(
                    new KeyValuePair<ExpressionType, string>( exp.NodeType, ((ConstantExpression)exp).Value.ToString()) 
                    );
            }
            else
            if (exp.NodeType == ExpressionType.Parameter)
            {
                parsedTree.Add(
                    new KeyValuePair<ExpressionType, string>(exp.NodeType, ((ParameterExpression)exp).Name)
                    );
            }
            else
            {
                switch (exp.NodeType)
                {
                    case ExpressionType.Add:
                        parsedTree.Add(
                        new KeyValuePair<ExpressionType, string>(exp.NodeType, "+")
                        );
                        break;
                    case ExpressionType.Divide:
                        parsedTree.Add(
                        new KeyValuePair<ExpressionType, string>(exp.NodeType, "/")
                        );
                        break;
                    case ExpressionType.Subtract:
                        parsedTree.Add(
                        new KeyValuePair<ExpressionType, string>(exp.NodeType, "-")
                        );
                        break;
                    case ExpressionType.Multiply:
                        parsedTree.Add(
                        new KeyValuePair<ExpressionType, string>(exp.NodeType, "*")
                        );
                        break;
                }

                
            }

            //Console.WriteLine(parsedTree.Last().Key.ToString() + " " + parsedTree.Last().Value.ToString());
        }

    }
}
