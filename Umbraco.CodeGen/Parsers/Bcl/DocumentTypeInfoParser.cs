﻿using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using Umbraco.CodeGen.Configuration;
using Umbraco.CodeGen.Definitions;

namespace Umbraco.CodeGen.Parsers.Bcl
{
    public class DocumentTypeInfoParser : CommonInfoParser
    {
        public DocumentTypeInfoParser(ContentTypeConfiguration configuration) : base(configuration)
        {
        }

        protected override void OnParseInfo(TypeDeclaration type, ContentType definition)
        {
            base.OnParseInfo(type, definition);

            var docType = (DocumentType)definition;
            var info = (DocumentTypeInfo)docType.Info;

            info.DefaultTemplate = StringFieldValue(type, "DefaultTemplate");
            info.AllowedTemplates = StringArrayValue(type, "AllowedTemplates").ToList();
        }
    }
}
