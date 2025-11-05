shader_type canvas_item;
render_mode unshaded; // Add this if you don't want 2D lights to affect it

// --- Uniform Variables for Wave and Gradient ---

uniform float wave_speed : hint_range(0.1, 180.0) = 4.0;
uniform float wave_frequency : hint_range(0.0, 180.0) = 20.0;
uniform float wave_intensity : hint_range(0.0, 1.0) = 0.1;

uniform float gradient_angle : hint_range(0.0, 360.0) = 90.0;
uniform sampler2D gradient_texture : hint_default_black;

void fragment() {

	// Convert gradient angle to radians
	float gradient_rads = radians(gradient_angle);
	vec2 gradient_direction = vec2(cos(gradient_rads), sin(gradient_rads));

    // Wave Distortion
    // Start with the base UV (UV)
    vec2 uv = UV;

    // Apply sine distortion to the x-coordinate

	vec2 normalized_direction = normalize(gradient_direction);
	vec2 centered_uv = UV - 0.5;

	uv.x += sin(uv.y * wave_frequency + TIME * wave_speed) * wave_intensity;
	centered_uv += sin(uv.y * wave_frequency + TIME * wave_speed) * wave_intensity;
	float gradient_factor = dot(centered_uv, normalized_direction);

    // 1. Get the texture/base color using the distorted UV
    // The texture() call implicitly multiplies the texture color by the particle's vertex COLOR
    // if COLOR is passed from a ParticlesProcessMaterial.
    vec4 base_data = texture(TEXTURE, uv) * COLOR; // Use 'uv' for distorted texture sampling

    // Color Gradient Implementation
    // Use UV.y as the blending factor (0.0 at the top, 1.0 at the bottom)
    // NOTE: Use the original UV.y for a smooth, straight gradient, not the distorted 'uv.y'
    gradient_factor = clamp(gradient_factor, 0.0, 1.0);

    // Mix the start and end colors based on the new factor
    //vec4 gradient_color = mix(color_start, color_end, gradient_factor);
	vec4 gradient_color = texture(gradient_texture, vec2(gradient_factor, 0.0));

    // Final Color Combination
    vec3 final_rgb = base_data.rgb * gradient_color.rgb;
    float final_alpha = base_data.a;
    COLOR = vec4(final_rgb, final_alpha);
}