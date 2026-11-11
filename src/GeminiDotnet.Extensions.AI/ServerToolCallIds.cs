namespace GeminiDotnet.Extensions.AI;

/// <summary>
/// Assigns the id that pairs a server-side call with its result. A call keeps the id Gemini issued, and an
/// id-less call gets a synthesized id that the next id-less result takes.
/// </summary>
internal sealed class ServerToolCallIds
{
    // The spec populates a result's id only when its call had one, so an id-less result can only answer an
    // id-less call. Queueing every call would let a result that names its call shift another call's place.
    //
    // A probe on 2026-11-11 found ids on every invocation from the Gemini 3.x models, with or without
    // includeServerSideToolInvocations, and none on code execution from gemini-2.5-flash, gemini-2.5-flash-lite
    // and gemini-2.5-pro, where each result followed its call. From 2026-12-01 the first and last of those
    // route to 3.x models, leaving gemini-2.5-flash-lite, which is served "until further notice". Once no
    // model omits ids, the queue can go and a missing id can simply be synthesized.
    private readonly Queue<string> _idLessCalls = [];

    public string ForCall(string? wireId, string prefix)
    {
        if (wireId is not null)
        {
            return wireId;
        }

        var callId = Synthesize(prefix);
        _idLessCalls.Enqueue(callId);
        return callId;
    }

    public string ForResult(string? wireId, string prefix) =>
        wireId ?? (_idLessCalls.TryDequeue(out var callId) ? callId : Synthesize(prefix));

    private static string Synthesize(string prefix) => $"{prefix}/{Guid.NewGuid()}";
}
