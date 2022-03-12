using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using ArknightsResources.Operators.Models;
using OperatorResources = ArknightsResources.Operators.Resources.Properties.Resources;

namespace ArknightsResources.Operators.Resources
{
    /// <summary>
    /// 为ArknightsResources.Operators.Resources的资源访问提供帮助的类
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// 通过干员的图片代号获取其图片
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="System.Resources.MissingManifestResourceException"/>
        /// <exception cref="System.Resources.MissingSatelliteAssemblyException"/>
        /// <param name="codename">干员的图片代号</param>
        /// <param name="type">干员的类型</param>
        /// <param name="skinInfo">干员的皮肤信息,该参数在<see cref="OperatorType"/>为<see cref="OperatorType.Skin"/>时才使用,其余时候请传递null</param>
        /// <returns>一个byte数组,其中包含了干员的图片信息</returns>
        public static byte[] GetOperatorImage(string codename, OperatorType type, OperatorSkinInfo? skinInfo)
        {
            if (string.IsNullOrWhiteSpace(codename))
            {
                throw new ArgumentException($"“{nameof(codename)}”不能为 null 或空白。", nameof(codename));
            }

            string operatorType = "1";
            switch (type)
            {
                case OperatorType.Elite0:
                    operatorType = "1";
                    break;
                case OperatorType.Elite1:
                    if (codename.Equals("amiya"))
                    {
                        return OperatorResources.operator_image_amiya_1_;
                    }
                    else
                    {
                        operatorType = "1";
                    }
                    break;
                case OperatorType.Elite2:
                    operatorType = "2";
                    break;
                case OperatorType.Skin:
                    operatorType = skinInfo != null
                        ? skinInfo.Value.ImageCodename
                        : throw new ArgumentNullException(nameof(skinInfo), "当OperatorType为Skin时,必须传递参数OperatorSkinInfo");
                    break;
                case OperatorType.Promotion:
                    operatorType = "promotion";
                    break;
                default:
                    break;
            }

            string name = $"operator_image_{codename}_{operatorType}";
            byte[] value = (byte[])OperatorResources.ResourceManager.GetObject(name);
            if (value is null)
            {
                throw new ArgumentException($@"使用给定的参数""{codename}"",""{operatorType}""和{(skinInfo.HasValue ? $@"""{skinInfo.Value}""" : "值为null的skinInfo")}""时找不到资源");
            }
            return value;
        }

        /// <summary>
        /// 通过干员对象获取其图片
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="System.Resources.MissingManifestResourceException"/>
        /// <exception cref="System.Resources.MissingSatelliteAssemblyException"/>
        /// <param name="op">干员对象</param>
        /// <param name="type">干员的类型</param>
        /// <param name="skinInfo">干员的皮肤信息,该参数在<see cref="OperatorType"/>为<see cref="OperatorType.Skin"/>时才使用,其余时候请传递null</param>
        /// <returns>一个byte数组,其中包含了干员的图片信息</returns>
        public static byte[] GetOperatorImage(Operator op, OperatorType type, OperatorSkinInfo? skinInfo)
        {
            string operatorType = "1";
            switch (type)
            {
                case OperatorType.Elite0:
                    operatorType = "1";
                    break;
                case OperatorType.Elite1:
                    if (op.ImageCodename.Equals("amiya"))
                    {
                        return OperatorResources.operator_image_amiya_1_;
                    }
                    else
                    {
                        operatorType = "1";
                    }
                    break;
                case OperatorType.Elite2:
                    operatorType = "2";
                    break;
                case OperatorType.Skin:
                    operatorType = skinInfo != null
                        ? skinInfo.Value.ImageCodename
                        : throw new ArgumentNullException(nameof(skinInfo), "当OperatorType为Skin时,必须传递参数OperatorSkinInfo");
                    break;
                case OperatorType.Promotion:
                    operatorType = "promotion";
                    break;
                default:
                    break;
            }

            string name = $"operator_image_{op.ImageCodename}_{operatorType}";
            byte[] value = (byte[])OperatorResources.ResourceManager.GetObject(name);
            if (value is null)
            {
                throw new ArgumentException($@"使用给定的参数""{op.Name}"",""{operatorType}""和{(skinInfo.HasValue ? $@"""{skinInfo.Value}""" : "值为null的skinInfo")}""时找不到资源");
            }
            return value;
        }

        /// <summary>
        /// 通过干员名称获取其<see cref="Operator"/>对象
        /// </summary>
        /// <param name="operatorName">干员名称</param>
        /// <param name="cultureInfo"><see cref="Operator"/>对象的语言文化</param>
        /// <returns>一个<see cref="Operator"/>对象</returns>
        /// <exception cref="ArgumentException"/>
        public static Operator GetOperator(string operatorName, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(operatorName))
            {
                throw new ArgumentException($"“{nameof(operatorName)}”不能为 null 或空白。", nameof(operatorName));
            }

            try
            {
                return GetOperatorInternal(operatorName, cultureInfo);
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"参数\"{operatorName}\"无效", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException($"提供的语言文化\"{cultureInfo}\"无效", ex);
            }
        }

        /// <summary>
        /// 通过干员名称获取其<see cref="Operator"/>对象
        /// </summary>
        /// <param name="operatorName">干员名称</param>
        /// <param name="cultureInfo"><see cref="Operator"/>对象的语言文化</param>
        /// <returns>一个<see cref="Operator"/>对象</returns>
        /// <exception cref="ArgumentException"/>
        public static async Task<Operator> GetOperatorAsync(string operatorName, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(operatorName))
            {
                throw new ArgumentException($"“{nameof(operatorName)}”不能为 null 或空白。", nameof(operatorName));
            }

            try
            {
                return await Task.Run(() => GetOperatorInternal(operatorName, cultureInfo));
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"参数\"{operatorName}\"无效", ex);
            }
        }

        private static Operator GetOperatorInternal(string operatorName, CultureInfo cultureInfo)
        {
            string opXmlStrings = OperatorResources.ResourceManager.GetString("Operators", cultureInfo);

            XDocument xDocument = XDocument.Parse(opXmlStrings);
            var operators = (from XElement element in xDocument.Root.Elements()
                             where element.Attribute("Name").Value == operatorName
                             select element).AsParallel();
            XElement opXML = operators.First();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Operator), "http://schema.livestudio.com/Operators.xsd");
            Operator op = (Operator)xmlSerializer.Deserialize(opXML.CreateReader());
            return op;
        }

        /// <summary>
        /// 获取全部干员
        /// </summary>
        /// <returns>一个<see cref="Operator"/>数组</returns>
        public static Operator[] GetAllOperators(CultureInfo cultureInfo)
        {
            return GetAllOperatorsInternal(cultureInfo);
        }

        /// <summary>
        /// 异步获取全部干员
        /// </summary>
        /// <returns>一个<see cref="Operator"/>数组</returns>
        public static async Task<Operator[]> GetAllOperatorsAsync(CultureInfo cultureInfo)
        {
            return await Task.Run(() => GetAllOperatorsInternal(cultureInfo));
        }

        private static Operator[] GetAllOperatorsInternal(CultureInfo cultureInfo)
        {
            string operatorsXmlString = OperatorResources.ResourceManager.GetString("Operators", cultureInfo);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(OperatorsList), "http://schema.livestudio.com/Operators.xsd");
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(operatorsXmlString)))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return ((OperatorsList)xmlSerializer.Deserialize(reader)).OperatorList;
                }
            }
        }
    }
}
