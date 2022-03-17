using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
  internal class AssemblyWrapper
  {
    #region Поля и свойства
    private readonly Type type;
    
    /// <summary>
    /// Объект типа класса
    /// </summary>
    public Type Type => type;

    private readonly object instance;
    
    /// <summary>
    /// Инстанс класса
    /// </summary>
    public object Instance => instance;
    #endregion

    #region Методы
    /// <summary>
    /// Испольняет статический метод класса
    /// </summary>
    /// <param name="methodName">Имя метода</param>
    /// <param name="args">Аргументы</param>
    /// <param name="result">Возрващаемое значение</param>
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
    /// Запускает не статически метод класса
    /// Необходимо наличие инстанса класса
    /// </summary>
    /// <param name="methodName">Имя метода</param>
    /// <param name="args">Аргументы</param>
    /// <param name="result">Возрващаемое значение</param>
    public void Run(string methodName, object[] args, out object result)
    {
      if (string.IsNullOrEmpty(methodName))
        throw new ArgumentNullException(nameof(methodName));

      if (this.instance == null) 
        throw new Exception("This is static class");

      var method = this.type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == args.Length);
      if (method == null) new Exception($"I don't find {method} method");

      result = method.Invoke(this.instance, args);
    }
    #endregion

    #region Конструкторы
    public AssemblyWrapper(string assemblyString, string typeName, bool isStatic)
    {
      var asm = Assembly.Load(assemblyString);
      if (asm == null) throw new Exception("I don't load assembly");

      this.type = asm.GetType(typeName);
      if (this.type == null) 
        throw new Exception($"I don't load {assemblyString} type from {typeName} assembly");

      if (!isStatic) 
      { 
        this.instance = Activator.CreateInstance(this.type);
        if (this.instance == null) 
          throw new Exception($"I don't create instance of {typeName}");
      }
    }
    #endregion
  }
}
