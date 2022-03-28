using System;
using System.Linq;
using System.Reflection;

namespace TypeWrapper
{

  /// <summary>
  /// Класс обертка для типа
  /// Упращает вызов методов типа из сборки.
  /// </summary>
  internal sealed class TypeWrapper
  {
    #region Поля и свойства

    /// <summary>
    /// Объект типа класса.
    /// </summary>
    private readonly Type type;

    #endregion

    #region Методы

    /// <summary>
    /// Испольняет статический метод класса.
    /// </summary>
    /// <param name="methodName">Имя метода.</param>
    /// <param name="args">Аргументы.</param>
    /// <param name="result">Возрващаемое значение.</param>
    public void RunStatic(string methodName, object[] args, out object result)
    {
      if (string.IsNullOrEmpty(methodName))
        throw new ArgumentNullException(nameof(methodName));

      var method = this.type.GetMethods(BindingFlags.Public | BindingFlags.Static)
        .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == args.Length);
      if (method == null)
        throw new Exception($"I don't find {method} method");

      result = method.Invoke(null, args);
    }

    /// <summary>
    /// Запускает не статически метод класса.
    /// </summary>
    /// <param name="methodName">Имя метода.</param>
    /// <param name="args">Аргументы.</param>
    /// <param name="result">Возрващаемое значение.</param>
    public void Run(string methodName, object[] args, out object result)
    {
      if (string.IsNullOrEmpty(methodName))
        throw new ArgumentNullException(nameof(methodName));

      var method = this.type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == args.Length);
      if (method == null)
        throw new Exception($"I don't find {method} method");

      var instance = Activator.CreateInstance(this.type);
      if (instance == null)
        throw new Exception($"Instance of type {type.Name} has not created.");

      result = method.Invoke(instance, args);
    }

    /// <summary>
    /// Возвращает публичные методы типа.
    /// </summary>
    /// <returns>Метаданные методов.</returns>
    public MethodInfo[] GetPublicMethods()
    {
      return type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// Загружает сборку и указанный тип.
    /// </summary>
    /// <param name="assemblyString">Строка с название сборки.</param>
    /// <param name="typeName">Название типа.</param>
    public TypeWrapper(string assemblyString, string typeName)
    {
      var asm = Assembly.Load(assemblyString);
      if (asm == null)
        throw new Exception("I don't load assembly");

      this.type = asm.GetType(typeName);
      if (this.type == null)
        throw new Exception($"I don't load {assemblyString} type from {typeName} assembly");
    }

    #endregion
  }
}
