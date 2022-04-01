using System;
using System.IO;
using System.Text;

namespace LazyBind
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
    /// Стандартная точка входа в приложение.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    public static void Main(string[] args)
    {
      try
      {
        var messageBox = new TypeWrapper(AssemblyString, TypeName);
        messageBox.RunStatic(MethodName, new[] { "Hello World!" });

        WriteToFile(messageBox);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// Запись имен публичных методов указанного типа в файл.
    /// </summary>
    /// <param name="typeWrapper">Обертка типа.</param>
    private static void WriteToFile(TypeWrapper typeWrapper)
    {
      using (var file = new StreamWriter(FileName, false))
      {
        var methodNames = new StringBuilder();
        foreach (var method in typeWrapper.GetPublicMethods())
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
