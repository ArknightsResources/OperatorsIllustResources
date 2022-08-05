using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;
using ArknightsResources.CustomResourceHelpers;
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
    public class ResourceHelper : OperatorResourceHelper
    {
        /// <summary>
        /// <seealso cref="ResourceHelper"/>的实例
        /// </summary>
        public static readonly ResourceHelper Instance = new ResourceHelper();

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="MissingManifestResourceException"/>
        /// <exception cref="MissingSatelliteAssemblyException"/>
        public override byte[] GetOperatorImage(OperatorIllustrationInfo illustrationInfo)
        {
            string name;
            string fileName = illustrationInfo.ImageCodename.Split('_')[0].Split('#')[0];
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
        public Image<Bgra32> GetOperatorImageReturnImage(OperatorIllustrationInfo illustrationInfo)
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

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"/>
        public override Operator GetOperator(string operatorName, CultureInfo cultureInfo)
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
        public async Task<Operator> GetOperatorAsync(string operatorName, CultureInfo cultureInfo)
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

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"/>
        public override Operator GetOperatorWithCodename(string operatorCodename, CultureInfo cultureInfo)
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
        public async Task<Operator> GetOperatorWithCodenameAsync(string operatorCodename, CultureInfo cultureInfo)
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

        /// <inheritdoc/>
        public override OperatorsList GetAllOperators(CultureInfo cultureInfo)
        {
            return GetAllOperatorsInternal(cultureInfo);
        }

        /// <inheritdoc/>
        public override async Task<OperatorsList> GetAllOperatorsAsync(CultureInfo cultureInfo)
        {
            return await Task.Run(() => GetAllOperatorsInternal(cultureInfo));
        }

        private static Operator GetOperatorInternal(string operatorName, CultureInfo cultureInfo)
        {
            byte[] opJson = (byte[])OperatorResources.ResourceManager.GetObject("operators", cultureInfo);
            Operator op = null;
            using (JsonDocument document = JsonDocument.Parse(opJson))
            {
                JsonElement root = document.RootElement;
                JsonElement operatorsElement = root.GetProperty("Operators");
                foreach (JsonProperty item in operatorsElement.EnumerateObject())
                {
                    bool enumComplete = false;
                    foreach (JsonProperty item2 in from JsonProperty item2 in item.Value.EnumerateObject()
                                                    where item2.Name == "Name" && item2.Value.GetString() == operatorName
                                                    select item2)
                    {
                        op = item.Value.Deserialize<Operator>(new JsonSerializerOptions()
                        {
                            WriteIndented = true,
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                            Converters =
                            {
                                new JsonStringEnumConverter()
                            }
                        });
                        enumComplete = true;
                        break;
                    }

                    if (enumComplete)
                    {
                        break;
                    }
                }
            }
            return op;
        }

        private static Operator GetOperatorWithCodenameInternal(string operatorImageCodename, CultureInfo cultureInfo)
        {
            byte[] opJson = (byte[])OperatorResources.ResourceManager.GetObject("operators", cultureInfo);
            Operator op = null;
            using (JsonDocument document = JsonDocument.Parse(opJson))
            {
                JsonElement root = document.RootElement;
                JsonElement operatorsElement = root.GetProperty("Operators");
                foreach (JsonProperty item in operatorsElement.EnumerateObject())
                {
                    bool enumComplete = false;
                    foreach (JsonProperty item2 in from JsonProperty item2 in item.Value.EnumerateObject()
                                                    where item2.Name == "ImageCodename" && item2.Value.GetString() == operatorImageCodename
                                                    select item2)
                    {
                        op = item.Value.Deserialize<Operator>(new JsonSerializerOptions()
                        {
                            WriteIndented = true,
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                            Converters =
                            {
                                new JsonStringEnumConverter()
                            }
                        });
                        enumComplete = true;
                        break;
                    }

                    if (enumComplete)
                    {
                        break;
                    }
                }
            }
            return op;
        }

        private static OperatorsList GetAllOperatorsInternal(CultureInfo cultureInfo)
        {
            byte[] operators = (byte[])OperatorResources.ResourceManager.GetObject("operators", cultureInfo);
            OperatorsList operatorsList = JsonSerializer.Deserialize<OperatorsList>(operators, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                Converters =
                            {
                                new JsonStringEnumConverter()
                            }
            });
            return operatorsList;
        }
    }
}
