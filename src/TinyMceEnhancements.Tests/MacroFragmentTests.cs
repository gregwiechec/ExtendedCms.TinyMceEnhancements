using System.Security.Principal;
using EPiServer.Core.Html.StringParsing;
using EPiServer.Security;
using ExtendedCms.TinyMceEnhancements.MacroVariables;
using ExtendedCms.TinyMceEnhancements.MacroVariables.MarcoVariables;
using FluentAssertions;
using Xunit;

namespace TinyMceEnhancements.Tests;

[Trait(nameof(MacroFragment), nameof(MacroFragment.GetViewFormat))]
public class MacroFragmentTests
{
    [Fact]
    public void When_there_is_no_macro_it_should_return_same_string()
    {
        var macroFragment = new MacroFragment(new StaticFragment("<p>abcd</p>"),
            new MacroVariablesOptions
            {
                Enabled = true,
                MacroPrefix = "%%%",
                MacroPostfix = "%%%"
            },
            new ITinyMceMacroVariable[] {new UserMacroVariable(new CustomPrincipalAccessor())});
        macroFragment.GetViewFormat().Should().Be("<p>abcd</p>");
    }

    [Fact]
    public void When_there_is_macro_it_should_return_converted_string()
    {
        var macroFragment = new MacroFragment(new StaticFragment("<p>abcd %%%USER_NAME%%% abcd</p>"),
            new MacroVariablesOptions
            {
                Enabled = true,
                MacroPrefix = "%%%",
                MacroPostfix = "%%%"
            },
            new ITinyMceMacroVariable[] {new UserMacroVariable(new CustomPrincipalAccessor())});
        macroFragment.GetViewFormat().Should().Be("<p>abcd TEST USER abcd</p>");
    }
    
    [Fact]
    public void When_there_are_two_same_macros_it_should_replace_two_macro_variables()
    {
        var macroFragment = new MacroFragment(new StaticFragment("<p>abcd %%%USER_NAME%%% %%%USER_NAME%%% abcd</p>"),
            new MacroVariablesOptions
            {
                Enabled = true,
                MacroPrefix = "%%%",
                MacroPostfix = "%%%"
            },
            new ITinyMceMacroVariable[] {new UserMacroVariable(new CustomPrincipalAccessor())});
        macroFragment.GetViewFormat().Should().Be("<p>abcd TEST USER TEST USER abcd</p>");
    }
}

public class CustomPrincipalAccessor : IPrincipalAccessor
{
    public IPrincipal Principal {
        get => new GenericPrincipal(new GenericIdentity("TEST USER"), Array.Empty<string>());
        set { }
    }
}