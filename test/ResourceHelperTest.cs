using ArknightsResources.CustomResourceHelpers;
using System.Globalization;
using ArknightsResources.Operators.Models;

namespace ArknightsResources.Operators.Resources.Test
{
    public class ResourceHelperTest
    {
        private readonly CultureInfo ChineseSimplifiedCultureInfo = OperatorResourceHelper.ChineseSimplifiedCultureInfo;

        [Fact]
        public async void GetAllOperatorTest()
        {
            var value = ResourceHelper.Instance.GetAllOperators(ChineseSimplifiedCultureInfo);
            var valueAsync = await ResourceHelper.Instance.GetAllOperatorsAsync(ChineseSimplifiedCultureInfo);
            Assert.NotEmpty(value.Operators);
            Assert.NotEmpty(valueAsync.Operators);
        }

        [Fact]
        public void GetOperatorImageTest()
        {
            OperatorIllustrationInfo operatorInfo = new("忒斯特收藏/I-报童", "LIMITED EDITION\n忒斯特™收藏系列/报童。限定区域供应。经典款式复刻，采用绝佳材质，附加特别缝纫细节。附带特别设计的开信刀。", "amiya_winter", OperatorType.Skin, "唯@W");
            byte[] value = ResourceHelper.Instance.GetOperatorImage(operatorInfo);
            Assert.NotEmpty(value);
        }

        [Fact]
        public void GetOperatorTest()
        {
            Operator op = ResourceHelper.Instance.GetOperator("阿米娅", ChineseSimplifiedCultureInfo);
            Assert.Equal("阿米娅", op.Name);
            Assert.NotEmpty(op.Profiles);
            Assert.NotEqual(0, op.Birthday!.Value.Month);
            Assert.NotEqual(0, op.Birthday.Value.Day);
        }

        [Fact]
        public void GetOperatorWithCodenameTest()
        {
            Operator op = ResourceHelper.Instance.GetOperatorWithImageCodename("gdglow", ChineseSimplifiedCultureInfo);
            Assert.Equal("澄闪", op.Name);
            Assert.NotEmpty(op.Profiles);
            Assert.NotEqual(0, op.Birthday!.Value.Month);
            Assert.NotEqual(0, op.Birthday.Value.Day);
        }

        [Fact]
        public async void GetOperatorAsyncTest()
        {
            Operator op = await ResourceHelper.Instance.GetOperatorAsync("阿米娅", ChineseSimplifiedCultureInfo);
            Assert.Equal("阿米娅", op.Name);
            Assert.NotEmpty(op.Profiles);
            Assert.NotEqual(0, op.Birthday!.Value.Month);
            Assert.NotEqual(0, op.Birthday.Value.Day);
        }

        [Fact]
        public async void GetOperatorWithCodenameAsyncTest()
        {
            Operator op = await ResourceHelper.Instance.GetOperatorWithImageCodenameAsync("gdglow", ChineseSimplifiedCultureInfo);
            Assert.Equal("澄闪", op.Name);
            Assert.NotEmpty(op.Profiles);
            Assert.NotEqual(0, op.Birthday!.Value.Month);
            Assert.NotEqual(0, op.Birthday.Value.Day);
        }
    }
}