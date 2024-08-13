using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApp18
{
    abstract class Format
    {
        public abstract string ToJsonOrXML(Message message);
        public abstract Message? FromJsonOrXML(string some);

    }

    class JSON : Format
    {
        public override Message? FromJsonOrXML(string some)
        {
            Message? result = null;
            result = JsonSerializer.Deserialize<Message>(some);
            return result;
        }

        public override string ToJsonOrXML(Message message)
        {
            string result = "";
            result = JsonSerializer.Serialize(message);
            return result;
        }
    }

    class XML : Format
    {
        public override Message? FromJsonOrXML(string some)
        {
            Message? message = null;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Message));
            using (StringReader reader = new StringReader(some)) 
            {
                message = xmlSerializer.Deserialize(reader) as Message;
            }

            return message;
        }

        public override string ToJsonOrXML(Message message)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Message));
            using (StringWriter writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, message);
                return  writer.ToString();
            }
        }
    }

    abstract class Creator
    {
        public abstract Format FactoryMethod();
    }

    class CreateJSON : Creator
    {
        public override Format FactoryMethod()
        {
            return new JSON();
        }
    }

    class CreateXml : Creator
    {
        public override Format FactoryMethod()
        {
            return new XML();
        }
    }
}
