namespace bfcs;

public class Interpreter 
{
    private string _source;
    private int[] _mem;
    private int _instPointer;
    private int _memPointer;
    private Stack<int> _addrStack;
    private string _input;
    private string _output;

    public Interpreter(string source)
    {
        this._source = source;
        this._mem = new int[100];
        this._instPointer = 0;
        this._memPointer = 0;
        this._addrStack = new Stack<int>();
        this._input = "";
        this._output = "";
    }

    private void SetOutput()
    {
        try 
        {
            char outputChar = Convert.ToChar(this._mem[this._memPointer]);
            this._output += outputChar;
        } 
        catch(Exception e)
        {
            System.Console.WriteLine(e);
        }
    }

    public string Interpret() 
    {
        bool eof = false;

        while(!eof) 
        {
            if(this._instPointer > this._mem.Length || this._instPointer < 0) 
            {
                eof = true;
                break;
            }

            char currentChar = this._source[this._instPointer];

            switch(currentChar) 
            {
                case '>':
                    this._memPointer++;
                    break;
                case '<':
                    if(this._memPointer > 0) {
                        this._memPointer--;
                    }
                    break;
                case '+':
                    this._mem[this._memPointer]++;
                    break;
                case '-':
                    this._mem[this._memPointer]--;
                    break;
                case '.':
                    this.SetOutput();
                    break;
                case '[':
                    if(this._mem[this._memPointer] != 0)
                    {
                        this._addrStack.Push(this._instPointer);
                    }
                    else 
                    {
                        int count = 1;
                        while(count > 0)
                        {
                            this._instPointer++;
                            char loopChar = this._source[this._instPointer]; 
                            if(loopChar == '[')
                            {
                                count++;
                            }
                            else if(loopChar == ']')
                            {
                                count--;
                            }
                        }
                    }
                    break;
                case ']':
                    try
                    {
                        this._instPointer = this._addrStack.Pop() - 1;
                    }
                    catch(Exception e) 
                    {
                        System.Console.WriteLine(e);
                    }
                    break;
            }

            this._instPointer++;
        }

        return this._output;
    }
}

class Program
{
    static void Main(string[] args)
    {
        string source = "++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.";
        Interpreter interpreter = new Interpreter(source);
        string result = interpreter.Interpret();
        System.Console.WriteLine(result);
    }
}
