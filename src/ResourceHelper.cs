using System;
using System.Collections.Generic;
using System.Globalization;
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
using OperatorResources = ArknightsResources.Operators.Resources.Properties.Resources;

namespace ArknightsResources.Operators.Resources
{
    /// <summary>
    /// 为ArknightsResources.Operators.Resources的资源访问提供帮助的类
    /// </summary>
#if NET7_0_OR_GREATER
    public class ResourceHelper : IOperatorResourceHelper
#else
    public class ResourceHelper : OperatorResourceHelper
#endif

    {
#if !NET7_0_OR_GREATER
        /// <summary>
        /// <seealso cref="ResourceHelper"/>的实例
        /// </summary>
        public static readonly ResourceHelper Instance = new ResourceHelper();
#endif

        /// <summary>
        /// 获取干员图片代号与干员代号的映射表
        /// </summary>
        /// <param name="cultureInfo">干员代号所用语言</param>
        /// <returns>Key为干员图片代号,Value为干员代号的Dictionary&lt;string, string&gt;</returns>
#if NET7_0_OR_GREATER
        public static Dictionary<string, string> GetOperatorImageCodenameMapping(CultureInfo cultureInfo)
#else
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static")]
        public Dictionary<string, string> GetOperatorImageCodenameMapping(CultureInfo cultureInfo)
#endif
        {
            byte[] stringByteArray = (byte[])OperatorResources.ResourceManager.GetObject("operator_image_codename_mapping", cultureInfo);
            string jsonString = Encoding.UTF8.GetString(stringByteArray);
            Dictionary<string, string> dict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
            return dict;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="MissingManifestResourceException"/>
        /// <exception cref="MissingSatelliteAssemblyException"/>
#if NET7_0_OR_GREATER
        public static byte[] GetOperatorImage(OperatorIllustrationInfo illustInfo)
#else
        public override byte[] GetOperatorImage(OperatorIllustrationInfo illustInfo)
#endif
        {
            string fileName;
            string imageCodename = illustInfo.ImageCodename.Split('_')[0].Split('#')[0];
            if (illustInfo.Type == OperatorType.Skin)
            {
                fileName = $"operator_image_skin_{imageCodename}";
            }
            else
            {
                fileName = $"operator_image_{imageCodename}";
            }

            byte[] abPack = OperatorResources.ResourceManager.GetObject(fileName) as byte[];
            if (abPack is null)
            {
                throw new ArgumentException($@"使用给定的参数""{illustInfo}""时找不到资源");
            }

            byte[] image = AssetBundleHelper.GetOperatorIllustration(abPack, illustInfo.ImageCodename, illustInfo.Type == OperatorType.Skin);
            return image;
        }

        /// <inheritdoc/>
#if NET7_0_OR_GREATER
        public static async Task<byte[]> GetOperatorImageAsync(OperatorIllustrationInfo illustInfo)
#else
        public override async Task<byte[]> GetOperatorImageAsync(OperatorIllustrationInfo illustInfo)
#endif
        {
            return await Task.Run(() => GetOperatorImage(illustInfo));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"/>
#if NET7_0_OR_GREATER
        public static Operator GetOperator(string operatorName, CultureInfo cultureInfo)
#else
        public override Operator GetOperator(string operatorName, CultureInfo cultureInfo)
#endif
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
            catch (JsonException ex)
            {
                throw new ArgumentException($"提供的语言文化\"{cultureInfo}\"无效", ex);
            }
        }

        /// <inheritdoc />
#if NET7_0_OR_GREATER
        public static async Task<Operator> GetOperatorAsync(string operatorName, CultureInfo cultureInfo)
#else
        public override async Task<Operator> GetOperatorAsync(string operatorName, CultureInfo cultureInfo)
#endif
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
            catch (JsonException ex)
            {
                throw new ArgumentException($"提供的语言文化\"{cultureInfo}\"无效", ex);
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"/>
#if NET7_0_OR_GREATER
        public static Operator GetOperatorWithImageCodename(string imageCodename, CultureInfo cultureInfo)
#else
        public override Operator GetOperatorWithImageCodename(string imageCodename, CultureInfo cultureInfo)
#endif
        {
            if (string.IsNullOrWhiteSpace(imageCodename))
            {
                throw new ArgumentException($"“{nameof(imageCodename)}”不能为 null 或空白。", nameof(imageCodename));
            }

            try
            {
                return GetOperatorWithCodenameInternal(imageCodename, cultureInfo);
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"参数\"{imageCodename}\"无效", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException($"提供的语言文化\"{cultureInfo}\"无效", ex);
            }
        }

        /// <inheritdoc/>
#if NET7_0_OR_GREATER
        public static async Task<Operator> GetOperatorWithImageCodenameAsync(string imageCodename, CultureInfo cultureInfo)
#else
        public override async Task<Operator> GetOperatorWithImageCodenameAsync(string imageCodename, CultureInfo cultureInfo)
#endif
        {
            if (string.IsNullOrWhiteSpace(imageCodename))
            {
                throw new ArgumentException($"“{nameof(imageCodename)}”不能为 null 或空白。", nameof(imageCodename));
            }

            try
            {
                return await Task.Run(() => GetOperatorWithCodenameInternal(imageCodename, cultureInfo));
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"参数\"{imageCodename}\"无效", ex);
            }
        }

        /// <inheritdoc/>
#if NET7_0_OR_GREATER
        public static OperatorsList GetAllOperators(CultureInfo cultureInfo)
#else
        public override OperatorsList GetAllOperators(CultureInfo cultureInfo)
#endif
        {
            return GetAllOperatorsInternal(cultureInfo);
        }

        /// <inheritdoc/>
#if NET7_0_OR_GREATER
        public static async Task<OperatorsList> GetAllOperatorsAsync(CultureInfo cultureInfo)
#else
        public override async Task<OperatorsList> GetAllOperatorsAsync(CultureInfo cultureInfo)
#endif
        {
            return await Task.Run(() => GetAllOperatorsInternal(cultureInfo));
        }

        /// <summary>
        /// 通过干员的立绘信息获取包含其立绘等的AssetBundle文件
        /// </summary>
        /// <param name="illustInfo">干员的立绘信息</param>
        /// <returns>一个byte数组,其中包含了AssetBundle文件的数据</returns>
        /// <exception cref="ArgumentException"/>
#if NET7_0_OR_GREATER
        public static byte[] GetAssetBundleFile(OperatorIllustrationInfo illustInfo)
#else
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static")]
        public byte[] GetAssetBundleFile(OperatorIllustrationInfo illustInfo)
#endif
        {
            string name;
            string fileName = illustInfo.ImageCodename.Split('_')[0].Split('#')[0];
            if (illustInfo.Type == OperatorType.Skin)
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
                throw new ArgumentException($@"使用给定的参数""{illustInfo}""时找不到资源");
            }

            return value;
        }

        private static Operator GetOperatorInternal(string operatorName, CultureInfo cultureInfo)
        {
            byte[] opJson = (byte[])OperatorResources.ResourceManager.GetObject("operators", cultureInfo);
            Operator op = null;
            using (JsonDocument document = JsonDocument.Parse(opJson))
            {
                JsonElement operatorsElement = document.RootElement.GetProperty("Operators");
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
                            Converters = { new JsonStringEnumConverter() }
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
                JsonElement operatorsElement = document.RootElement.GetProperty("Operators");
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
                            Converters = { new JsonStringEnumConverter() }
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
                Converters = { new JsonStringEnumConverter() }
            });
            return operatorsList;
        }
    }
}
