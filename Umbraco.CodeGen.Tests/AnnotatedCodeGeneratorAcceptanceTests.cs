﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Umbraco.CodeGen.Configuration;
using Umbraco.CodeGen.Definitions;
using Umbraco.CodeGen.Generators;
using Umbraco.CodeGen.Generators.Annotated;
using Umbraco.CodeGen.Tests.TestHelpers;

namespace Umbraco.CodeGen.Tests
{
    [TestFixture]
    public class AnnotatedCodeGeneratorAcceptanceTests
    {
        [Test]
        public void BuildCode_GeneratesCodeForDocumentType()
        {
            TestBuildCode("SomeAnnotatedDocumentType", "SomeDocumentType", "DocumentType");
        }

        [Test]
        public void BuildCode_GeneratesCodeForMediaType()
        {
            TestBuildCode("SomeAnnotatedMediaType", "SomeMediaType", "MediaType");
        }

        private void TestBuildCode(string classFileName, string xmlFileName, string contentTypeName)
        {
            ContentType contentType;
            var expectedOutput = "";
            using (var inputReader = File.OpenText(@"..\..\TestFiles\" + xmlFileName + ".xml"))
            {
                contentType = new ContentTypeSerializer().Deserialize(inputReader);
            }
            using (var goldReader = File.OpenText(@"..\..\TestFiles\" + classFileName + ".cs"))
            {
                expectedOutput = goldReader.ReadToEnd();
            }

            var configuration = new CodeGeneratorConfiguration();
            configuration.TypeMappings.Add(new TypeMapping("1413afcb-d19a-4173-8e9a-68288d2a73b8", "Int32"));
            var typeConfig = configuration.Get(contentTypeName);
            typeConfig.BaseClass = "Umbraco.Core.Models.TypedModelBase";
            typeConfig.Namespace = "Umbraco.CodeGen.Models";

            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            var dataTypeProvider = new TestDataTypeProvider();
            var generator = new CodeGenerator(typeConfig, dataTypeProvider, new AnnotatedCodeGeneratorFactory());

            generator.Generate(contentType, writer);

            writer.Flush();
            Console.WriteLine(sb.ToString());

            Assert.AreEqual(expectedOutput, sb.ToString());
        }
    }
}