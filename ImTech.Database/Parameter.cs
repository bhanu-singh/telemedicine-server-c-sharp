using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.DataBase
{
    public class Parameter
    {
        public Parameter()
        {

        }

        public Parameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public Parameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public Parameter(string name, ParameterDirection direction)
        {
            Name = name;
            Direction = direction;
        }

        public Parameter(string name, string value, ParameterDirection direction)
        {
            Name = name;
            Value = value;
            Direction = direction;
        }

        public Parameter(string name, object value, ParameterDirection direction)
        {
            Name = name;
            Value = value;
            Direction = direction;
        }

        /// <summary>
        /// In Case of Parameter size is non zero, DBType Value must be provided
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="direction"></param>
        /// <param name="type"></param>
        /// <param name="size"></param>
        public Parameter(string name, object value, ParameterDirection direction, DbType type, int size = 0)
        {
            Name = name;
            Value = value;
            Direction = direction;
            Type = type;
            Size = size;
        }

        public string Name { get; set; }

        public object Value { get; set; }

        public ParameterDirection Direction { get; set; }

        public DbType Type { get; set; }

        public int Size { get; set; }
    }
}
