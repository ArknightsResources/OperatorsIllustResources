using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using ArknightsResources.Operators.Models;
using ArknightsResources.Utility;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using OperatorResources = ArknightsResources.Operators.Resources.Properties.Resources;

namespace ArknightsResources.Operators.Resources
{
    /// <summary>
    /// 为ArknightsResources.Operators.Resources的资源访问提供帮助的类
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// 通过干员的立绘信息获取其图片
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="System.Resources.MissingManifestResourceException"/>
        /// <exception cref="System.Resources.MissingSatelliteAssemblyException"/>
        /// <param name="illustrationInfo">干员的立绘信息</param>
        /// <returns>一个byte数组,其中包含了干员的图片信息</returns>
        public static byte[] GetOperatorImage(OperatorIllustrationInfo illustrationInfo)
        {
            string name;
            string fileName = illustrationInfo.ImageCodename.Split('_')[0];
            if (illustrationInfo.Type == OperatorType.Skin)
            {
                name = $"operator_image_skin_{fileName}";
            }
            else
            {
                name = $"operator_image_{fileName}";
            }

            byte[] value = (byte[])OperatorResources.ResourceManager.GetObject(name);
            if (value is null)
            {
                throw new ArgumentException($@"使用给定的参数""{illustrationInfo}""时找不到资源");
            }

            byte[] image = AssetBundleHelper.GetOperatorIllustration(value, illustrationInfo);
            return image;
        }

        /// <summary>
        /// 通过干员的立绘信息获取其图片
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="System.Resources.MissingManifestResourceException"/>
        /// <exception cref="System.Resources.MissingSatelliteAssemblyException"/>
        /// <param name="illustrationInfo">干员的立绘信息</param>
        /// <returns>一个Image对象,其中包含了干员的图片信息</returns>
        public static Image<Bgra32> GetOperatorImageReturnImage(OperatorIllustrationInfo illustrationInfo)
        {
            string name;
            string fileName = illustrationInfo.ImageCodename.Split('_')[0];
            if (illustrationInfo.Type == OperatorType.Skin)
            {
                name = $"operator_image_skin_{fileName}";
            }
            else
            {
                name = $"operator_image_{fileName}";
            }

            byte[] value = (byte[])OperatorResources.ResourceManager.GetObject(name);
            if (value is null)
            {
                throw new ArgumentException($@"使用给定的参数""{illustrationInfo}""时找不到资源");
            }

            Image<Bgra32> image = AssetBundleHelper.GetOperatorIllustrationReturnImage(value, illustrationInfo);
            return image;
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

        /// <summary>
        /// 通过干员图像代号获取其<see cref="Operator"/>对象
        /// </summary>
        /// <param name="operatorCodename">干员图像代号</param>
        /// <param name="cultureInfo"><see cref="Operator"/>对象的语言文化</param>
        /// <returns>一个<see cref="Operator"/>对象</returns>
        /// <exception cref="ArgumentException"/>
        public static Operator GetOperatorWithCodename(string operatorCodename, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(operatorCodename))
            {
                throw new ArgumentException($"“{nameof(operatorCodename)}”不能为 null 或空白。", nameof(operatorCodename));
            }

            try
            {
                return GetOperatorWithCodenameInternal(operatorCodename, cultureInfo);
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"参数\"{operatorCodename}\"无效", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException($"提供的语言文化\"{cultureInfo}\"无效", ex);
            }
        }

        /// <summary>
        /// 通过干员名称获取其<see cref="Operator"/>对象
        /// </summary>
        /// <param name="operatorCodename">干员名称</param>
        /// <param name="cultureInfo"><see cref="Operator"/>对象的语言文化</param>
        /// <returns>一个<see cref="Operator"/>对象</returns>
        /// <exception cref="ArgumentException"/>
        public static async Task<Operator> GetOperatorWithCodenameAsync(string operatorCodename, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(operatorCodename))
            {
                throw new ArgumentException($"“{nameof(operatorCodename)}”不能为 null 或空白。", nameof(operatorCodename));
            }

            try
            {
                return await Task.Run(() => GetOperatorWithCodenameInternal(operatorCodename, cultureInfo));
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"参数\"{operatorCodename}\"无效", ex);
            }
        }

        private static Operator GetOperatorInternal(string operatorCodename, CultureInfo cultureInfo)
        {
            string opXmlStrings = OperatorResources.ResourceManager.GetString("Operators", cultureInfo);

            XDocument xDocument = XDocument.Parse(opXmlStrings);
            var operators = (from XElement element in xDocument.Root.Elements()
                             where element.Attribute("Name").Value == operatorCodename
                             select element).AsParallel();
            XElement opXML = operators.First();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Operator), "http://schema.livestudio.com/Operators.xsd");
            Operator op = (Operator)xmlSerializer.Deserialize(opXML.CreateReader());
            return op;
        }

        private static Operator GetOperatorWithCodenameInternal(string operatorCodename, CultureInfo cultureInfo)
        {
            string opXmlStrings = OperatorResources.ResourceManager.GetString("Operators", cultureInfo);

            XDocument xDocument = XDocument.Parse(opXmlStrings);
            var operators = (from XElement element in xDocument.Root.Elements()
                             where element.Attribute("ImageCodename").Value == operatorCodename
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
