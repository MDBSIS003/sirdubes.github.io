using System.Text.Json;
using System.Text.Json.Nodes;
using FuturePath.Business;
using FuturePath.Fixtures;

namespace FuturePath.Tests;
public class OriginalScenarioRegressionTests
{
    [Theory]
    [InlineData(1)] [InlineData(2)] [InlineData(3)] [InlineData(4)] [InlineData(5)]
    [InlineData(6)] [InlineData(7)] [InlineData(8)] [InlineData(9)] [InlineData(10)]
    public void EverySeedRetainsItsFullOriginalScoresExplanationsAndRanking(int id)
    {
        ReferenceFixture fixture = new ReferenceFixture(Path.Combine(AppContext.BaseDirectory, "sample-data.json"));
        JsonNode baseline = JsonNode.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "expected-engine-results.json")))!;
        JsonNode expected = baseline[id.ToString()]!;
        RecommendationEngine engine = new RecommendationEngine();
        AdvisorySession session = engine.Assess(fixture.Input(id), fixture.Programmes(), fixture.Questions(), new DateTime(2026, 10, 18, 10, 0, 0, DateTimeKind.Utc));
        JsonNode actual = JsonSerializer.SerializeToNode(new { session.Selected, session.Alternatives, session.InterestScores })!;
        NormaliseLineEndings(expected);
        NormaliseLineEndings(actual);
        Assert.True(JsonNode.DeepEquals(expected, actual), "A supplied seed's score, explanation, outcome or alternative ranking changed.");
    }
    private static void NormaliseLineEndings(JsonNode node)
    {
        node["Selected"]!["Explanation"] = node["Selected"]!["Explanation"]!.GetValue<string>().Replace("\r\n", "\n");
        foreach (JsonNode? alternative in node["Alternatives"]!.AsArray())
        { alternative!["Explanation"] = alternative["Explanation"]!.GetValue<string>().Replace("\r\n", "\n"); }
    }
}
