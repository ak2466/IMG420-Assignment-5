shader_type canvas_item;
render_mode unshaded; // Add this if you don't want 2D lights to affect it

// --- Uniform Variables for Wave and Gradient ---

uniform float wave_speed : hint_range(0.1, 10.0) = 4.0;
uniform float wave_frequency : hint_range(1.0, 50.0) = 20.0;
uniform float wave_intensity : hint_range(0.0, 1.0) = 0.1;

uniform vec4 color_start : source_color = vec4(1.0, 0.5, 0.0, 1.0); // Orange-ish
uniform vec4 color_end : source_color = vec4(1.0, 0.0, 0.5, 1.0);   // Pink-ish

void fragment() {
    // 1. Get the texture (if any) and the vertex color (COLOR)
    // COLOR is the color passed from the ParticlesProcessMaterial
    vec4 base_data = texture(TEXTURE, UV) * COLOR;

    // --- Wave Distortion Implementation ---
    vec2 uv = UV;

    // Apply sinusoidal distortion to the x-coordinate based on:
    // - uv.y: Vertical position (to create horizontal waves)
    // - wave_frequency: How many waves across the height (uv.y * frequency)
    // - TIME * wave_speed: Animation speed
    // - wave_intensity: The magnitude of the distortion
    uv.x += sin(uv.y * wave_frequency + TIME * wave_speed) * wave_intensity;

    // Re-sample the texture/base color using the distorted UV
    base_data = texture(TEXTURE, uv) * COLOR;

    // --- Color Gradient Implementation ---
    // Use UV.y as the blending factor (0.0 at the top, 1.0 at the bottom)
    float gradient_factor = uv.y;

    // Mix the start and end colors based on the vertical position (gradient_factor)
    vec4 final_color = mix(color_start, color_end, gradient_factor);

    // Use the base_data (distorted texture/color) for color or combine as needed.
    // Here, we'll use the final_color for the main effect and multiply its alpha by the texture alpha.

    // To apply the color gradient *to* the texture:
    final_color.rgb *= base_data.rgb; // Apply gradient color to the texture/base color

    // To use the alpha from the original texture/particle data:
    final_color.a = base_data.a;

    // 4. Output the final color
    COLOR = texture(TEXTURE, uv);
}