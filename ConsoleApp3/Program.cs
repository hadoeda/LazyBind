using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApp3
{
  class Program
  {
    private const string AssemblyString = "System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
    
    private const string TypeName = "System.Windows.Forms.MessageBox";

    private const string MethodName = "Show";

    private const string FileName = "fields.txt";

    static void Main(string[] args)
    {
      var messageBox = new AssemblyWrapper(AssemblyString, TypeName, true);
      messageBox.RunStatic(MethodName, new[] { "Hellow World!" }, out object result);
      
      WriteToFile(messageBox.Type);
    }

    static void WriteToFile(Type type)
    {
      using(var stream = new FileStream(FileName, FileMode.Create))
      using (var file = new StreamWriter(stream))
      {
        var methodNames = new StringBuilder();
        foreach(var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
        {
          methodNames.Append(method.Name);
          methodNames.AppendLine();
        }

        file.Write(methodNames);
        file.Flush();
      }
    }
  }
}
