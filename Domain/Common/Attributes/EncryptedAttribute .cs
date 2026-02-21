using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptedAttribute : Attribute
    {
    }
}
