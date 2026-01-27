shader_type canvas_item;

uniform vec4 line_color : source_color = vec4(1.0, 1.0, 1.0, 0.2);
uniform vec4 deep_water_color : source_color = vec4(0.0, 0.05, 0.1, 1.0);
uniform float pixel_size : hint_range(1.0, 10.0) = 1.0;
uniform float wave_speed : hint_range(0.0, 5.0) = 0.4;
uniform float base_scale : hint_range(1.0, 500.0) = 150.0;
uniform float line_sharpness : hint_range(0.0, 1.0) = 0.96;

uniform float top_fade : hint_range(-1.0, 1.0) = -0.2;   
uniform float bottom_fade : hint_range(0.0, 2.0) = 1.2; 
uniform float bg_darkness : hint_range(0.0, 1.0) = 0.6;

void vertex() {
	float stepped_time = round(TIME * 8.0) / 8.0;
	float wobble = sin(stepped_time * wave_speed + VERTEX.y * 0.05) * 2.0;
	VERTEX.x += round(wobble);
}

void fragment() {
	vec2 res = vec2(textureSize(TEXTURE, 0));
	vec2 grid_uv = round(UV * (res / pixel_size)) / (res / pixel_size);
	float t = round(TIME * 8.0) / 8.0;
	
	// 1. Asymmetric Wave Math
	// We vary the multipliers for x and y so they aren't perfect 45-degree angles
	// Wave 1: Squashed horizontally
	float wave1 = sin((grid_uv.x * 1.3 + grid_uv.y * 0.8 + t * wave_speed) * base_scale);
	// Wave 2: Stretched vertically and moving at a different angle
	float wave2 = sin((grid_uv.x * 0.7 - grid_uv.y * 1.5 + t * (wave_speed * 0.5)) * (base_scale * 1.2));
	// Wave 3: Wide and slow for "clumpiness"
	float wave3 = sin((grid_uv.x * 1.1 + grid_uv.y * 0.3 + t * (wave_speed * 0.3)) * (base_scale * 0.6));
	
	// Combine them. By adding and subtracting, we get much more varied shapes.
	float combined = (wave1 + wave2 - wave3) / 3.0;
	
	// 2. Thresholding
	float reflection_mask = step(line_sharpness, combined);
	
	// 3. Depth & Color
	float depth_factor = clamp((grid_uv.y - top_fade) / (bottom_fade - top_fade), 0.0, 1.0);
	float reflection_fade = 1.0 - depth_factor;
	
	vec4 tex_color = texture(TEXTURE, grid_uv);
	vec3 ambient_bg = mix(tex_color.rgb, deep_water_color.rgb, depth_factor * bg_darkness);
	
	vec3 final_rgb = mix(ambient_bg, line_color.rgb, reflection_mask * line_color.a * reflection_fade);
	
	COLOR = vec4(final_rgb, tex_color.a);
}