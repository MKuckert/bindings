// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Bindings.Core.Binding.Bindings.SourceSteps
{
    public class LiteralSourceStepFactory : TypedSourceStepFactory<LiteralSourceStepDescription>
    {
        protected override ISourceStep TypedCreate(LiteralSourceStepDescription description)
        {
            var toReturn = new LiteralSourceStep(description);
            return toReturn;
        }
    }
}