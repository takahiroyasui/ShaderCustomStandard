using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Toguchi.Rendering
{
    [Serializable]
    [PostProcess(typeof(FlareRenderer), PostProcessEvent.AfterStack, "Custom/Flare")]
    public class Flare : PostProcessEffectSettings {
        public ColorParameter flareColor = new() { value = Color.black };
        public Vector2Parameter flarePosition = new() { value = Vector2.zero };
        public FloatParameter flareSize = new() { value = 0f };

        public ColorParameter paraColor = new() { value = Color.white};
        public Vector2Parameter paraPosition = new() { value = Vector2.zero };
        public FloatParameter paraSize = new() { value = 0f };
    }

    public sealed class FlareRenderer : PostProcessEffectRenderer<Flare> {
        public override void Render(PostProcessRenderContext context) {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Flare"));
            sheet.properties.SetVector("_FlareVector", new Vector4(settings.flarePosition.value.x, settings.flarePosition.value.y, 1f / settings.flareSize.value));
            sheet.properties.SetColor("_FlareColor", settings.flareColor.value);
            sheet.properties.SetVector("_ParaVector", new Vector4(settings.paraPosition.value.x, settings.paraPosition.value.y, 1f / settings.paraSize.value));
            sheet.properties.SetColor("_ParaColor", settings.paraColor.value);
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}