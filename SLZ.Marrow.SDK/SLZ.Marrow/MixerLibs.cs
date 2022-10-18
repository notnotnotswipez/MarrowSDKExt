using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Maranara.Marrow
{
    public static class MixerLibs
    {
        public static ClassDeclarationSyntax UpdateMainClass(CompilationUnitSyntax root, string @class)
        {
            return (ClassDeclarationSyntax)(root
    .ChildNodes().First().ChildNodes()
    .FirstOrDefault((c)
    => c.GetType() == typeof(ClassDeclarationSyntax)
    && ((ClassDeclarationSyntax)c).Identifier.ToString() == @class
    ) ?? root
    .ChildNodes()
    .FirstOrDefault((c)
    => c.GetType() == typeof(ClassDeclarationSyntax)
    && ((ClassDeclarationSyntax)c).Identifier.ToString() == @class
    ));

        }

        // https://stackoverflow.com/a/37743242
        public static MethodDeclarationSyntax GetMethodDeclarationSyntax(string returnTypeName, string methodName, string[] parameterTypes, string[] paramterNames)
        {
            var parameterList = ParameterList(SeparatedList(GetParametersList(parameterTypes, paramterNames)));
            return MethodDeclaration(attributeLists: List<AttributeListSyntax>(),
                          modifiers: TokenList(),
                          returnType: ParseTypeName(returnTypeName),
                          explicitInterfaceSpecifier: null,
                          identifier: Identifier(methodName),
                          typeParameterList: null,
                          parameterList: parameterList,
                          constraintClauses: List<TypeParameterConstraintClauseSyntax>(),
                          body: null,
                          semicolonToken: Token(SyntaxKind.SemicolonToken));

            IEnumerable<ParameterSyntax> GetParametersList(string[] parameterTypesToGet, string[] paramterNamesToGet)
            {
                for (int i = 0; i < parameterTypesToGet.Length; i++)
                {
                    yield return Parameter(attributeLists: List<AttributeListSyntax>(),
                                                             modifiers: TokenList(),
                                                             type: ParseTypeName(parameterTypesToGet[i]),
                                                             identifier: Identifier(paramterNamesToGet[i]),
                                                             @default: null);
                }
            }
        }

        public static string MakeAsmSafe(string _str)
        {
            string str = _str;
            // this wont work in some specific edge cases but for the most part it should be fine
            foreach (char c in Path.GetInvalidFileNameChars())
                str = str.Replace(c, '_');

            str = str.Trim('_');
            return str;
        }

        // modified from https://gist.github.com/xdaDaveShaw/87643170e5fa97b7da3b
        public class AttributeRemoverRewriter : CSharpSyntaxRewriter
        {
            public AttributeRemoverRewriter()
            {
            }

            public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
            {
                var newAttributes = new SyntaxList<AttributeListSyntax>();

                foreach (var attributeList in node.AttributeLists)
                {
                    var nodesToRemove =
                        attributeList
                        .Attributes
                        .ToArray();

                    if (nodesToRemove.Length != attributeList.Attributes.Count)
                    {
                        //We want to remove only some of the attributes
                        var newAttribute =
                            (AttributeListSyntax)VisitAttributeList(
                                attributeList.RemoveNodes(nodesToRemove, SyntaxRemoveOptions.KeepNoTrivia));

                        newAttributes = newAttributes.Add(newAttribute);
                    }
                }

                //Get the leading trivia (the newlines and comments)
                var leadTriv = node.GetLeadingTrivia();
                node = node.WithAttributeLists(newAttributes);

                //Append the leading trivia to the method
                node = node.WithLeadingTrivia(leadTriv);
                return node;
            }

            public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                var newAttributes = new SyntaxList<AttributeListSyntax>();

                foreach (var attributeList in node.AttributeLists)
                {
                    var nodesToRemove =
                        attributeList
                        .Attributes
                        .ToArray();

                    if (nodesToRemove.Length != attributeList.Attributes.Count)
                    {
                        //We want to remove only some of the attributes
                        var newAttribute =
                            (AttributeListSyntax)VisitAttributeList(
                                attributeList.RemoveNodes(nodesToRemove, SyntaxRemoveOptions.KeepNoTrivia));

                        newAttributes = newAttributes.Add(newAttribute);
                    }
                }

                //Get the leading trivia (the newlines and comments)
                var leadTriv = node.GetLeadingTrivia();
                node = node.WithAttributeLists(newAttributes);

                //Append the leading trivia to the method
                node = node.WithLeadingTrivia(leadTriv);
                return node;
            }
        }
    }
}
