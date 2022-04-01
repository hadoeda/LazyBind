using System;
using System.Linq;
using System.Reflection;

namespace LazyBind
{
  /// <summary>
  /// Обёртка типа для вызова его методов.
  /// </summary>
  internal sealed class TypeWrapper
  {
    #region Поля и свойства

    /// <summary>
    /// Оборачиваемый тип.
    /// </summary>
    private readonly Type type;

    #endregion

    #region Методы

    /// <summary>
    /// Испольняет статический метод класса.
    /// </summary>
    /// <param name="methodName">Имя метода.</param>
    /// <param name="args">Аргументы.</param>
    /// <returns>Возвращаемое методом значение.</returns>
    /// <exception cref="ArgumentNullException">Имя метода пустое.</exception>
    /// <exception cref="MissingMethodException">Метод не найден.</exception>
    public object RunStatic(string methodName, object[] args)
    {
      if (string.IsNullOrEmpty(methodName))
        throw new ArgumentNullException(nameof(methodName));

      var method = this.type.GetMethods(BindingFlags.Public | BindingFlags.Static)
        .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == args.Length);
      if (method == null)
        throw new MissingMethodException($"Мethod {methodName} is not found");

      return method.Invoke(null, args);
    }

    /// <summary>
    /// Возвращает публичные методы типа.
    /// </summary>
    /// <returns>Метаданные методов.</returns>
    public MethodInfo[] GetPublicMethods()
    {
      return this.type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// Загружает сборку и указанный тип.
    /// </summary>
    /// <param name="assemblyString">Строка с название сборки.</param>
    /// <param name="typeName">Название типа.</param>
    /// <exception cref="TypeLoadException">Не удалось загрузить тип.</exception>
    public TypeWrapper(string assemblyString, string typeName)
    {
      var asm = Assembly.Load(assemblyString);
      this.type = asm.GetType(typeName);
      if (this.type == null)
        throw new TypeLoadException($"Type {typeName} is not loaded from  assembly {assemblyString}");
    }

    #endregion
  }
}
