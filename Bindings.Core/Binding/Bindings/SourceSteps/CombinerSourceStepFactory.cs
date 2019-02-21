// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Bindings.Core.Binding.Bindings.SourceSteps
{
    public class CombinerSourceStepFactory : TypedSourceStepFactory<CombinerSourceStepDescription>
    {
        protected override ISourceStep TypedCreate(CombinerSourceStepDescription description)
        {
            var toReturn = new CombinerSourceStep(description);
            return toReturn;
        }
    }
}