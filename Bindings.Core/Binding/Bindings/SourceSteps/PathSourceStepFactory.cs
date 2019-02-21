// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Bindings.Core.Binding.Bindings.SourceSteps
{
    public class PathSourceStepFactory : TypedSourceStepFactory<PathSourceStepDescription>
    {
        protected override ISourceStep TypedCreate(PathSourceStepDescription description)
        {
            return new PathSourceStep(description);
        }
    }
}