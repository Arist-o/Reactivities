using MediatR;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Application.Core
{
    public class CommandRegistry
    {
        private static readonly Dictionary<string, Type> _commands = new(StringComparer.OrdinalIgnoreCase);

        public static void RegisterAllCommands(Assembly assembly)
        {
            var requestType = typeof(IBaseRequest); 

            var commandTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && requestType.IsAssignableFrom(t));

            foreach (var type in commandTypes)
            {
                string methodName = type.IsNested ? type.DeclaringType.Name : type.Name;

                _commands[methodName] = type;

            }
        }
        public static IEnumerable<string> GetRegisteredKeys()
        {
            return _commands.Keys;
        }

        public static Type GetCommandType(string methodName)
        {
            _commands.TryGetValue(methodName, out var type);
            return type;
        }
    }
}
