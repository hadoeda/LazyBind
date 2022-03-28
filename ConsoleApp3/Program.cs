using System;
using System.IO;
using System.Linq;
using System.Text;

namespace TypeWrapper
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
      var messageBox = new TypeWrapper(AssemblyString, TypeName);
      messageBox.RunStatic(MethodName, new[] { "Hello World!" }, out object result);

      WriteToFile(messageBox);
    }

    /// <summary>
    /// Запись имен публичных методов указанного типа в файл.
    /// </summary>
    /// <param name="type">Обертка типа</param>
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
