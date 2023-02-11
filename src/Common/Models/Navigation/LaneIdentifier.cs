using System;

namespace LMTS.Common.Models.Navigation;

public record struct LaneIdentifier(Guid Path, int Lane);