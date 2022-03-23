using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApp3
{
  /// <summary>
  /// Основной класс приложения.
  /// </summary>
  public class Program
  {
    #region Константы

    /// <summary>
    /// Строка с название сборки. 
    /// </summary>
    private const string AssemblyString = "System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
    
    /// <summary>
    /// Имя типа.
    /// </summary>
    private const string TypeName = "System.Windows.Forms.MessageBox";

    /// <summary>
    /// Название метода.
    /// </summary>
    private const string MethodName = "Show";

    /// <summary>
    /// Название файла.
    /// </summary>
    private const string FileName = "fields.txt";

    #endregion

    #region Методы

    /// <summary>
    /// Стандартная точка входа в приложние.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    public static void Main(string[] args)
    {
      var messageBox = new AssemblyWrapper(AssemblyString, TypeName, true);
      messageBox.RunStatic(MethodName, new[] { "Hellow World!" }, out object result);
      
      WriteToFile(messageBox.Type);
    }

    /// <summary>
    /// Запись имен публичных методов указанного типа в файл.
    /// </summary>
    /// <param name="type">Тип.</param>
    private static void WriteToFile(Type type)
    {
      using (var file = new StreamWriter(FileName, false))
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

    #endregion
  }
}
