shader_type canvas_item;

uniform vec4 line_color : source_color = vec4(1.0, 1.0, 1.0, 0.2);
uniform vec4 deep_water_color : source_color = vec4(0.0, 0.05, 0.1, 1.0);
uniform float pixel_size : hint_range(1.0, 10.0) = 1.0;
uniform float wave_speed : hint_range(0.0, 5.0) = 0.4;
uniform float base_scale : hint_range(1.0, 1000.0) = 150.0;
uniform float line_sharpness : hint_range(0.0, 1.0) = 0.96;

uniform float top_fade : hint_range(-1.0, 1.0) = -0.2;   
uniform float bottom_fade : hint_range(0.0, 2.0) = 1.2; 
uniform float bg_darkness : hint_range(0.0, 1.0) = 0.6;

// New uniform to track parallax movement
uniform vec2 scroll_offset = vec2(0.0);

void fragment() {
	// 1. USE SCREEN_UV INSTEAD OF UV
	// This makes the pattern relative to the screen, not the individual sprite.
	// We add 'scroll_offset' to make the pattern move with the parallax layer.
	vec2 world_uv = SCREEN_UV + (scroll_offset * 0.001); 
	
	// 2. Pixelate based on screen resolution
	vec2 screen_res = 1.0 / SCREEN_PIXEL_SIZE;
	vec2 grid_uv = round(world_uv * (screen_res / pixel_size)) / (screen_res / pixel_size);
	
	float t = round(TIME * 8.0) / 8.0;
	
	// 3. The same organic wave math from before
	float wave1 = sin((grid_uv.x * 1.3 + grid_uv.y * 0.8 + t * wave_speed) * base_scale);
	float wave2 = sin((grid_uv.x * 0.7 - grid_uv.y * 1.5 + t * (wave_speed * 0.5)) * (base_scale * 1.2));
	float wave3 = sin((grid_uv.x * 1.1 + grid_uv.y * 0.3 + t * (wave_speed * 0.3)) * (base_scale * 0.6));
	
	float combined = (wave1 + wave2 - wave3) / 3.0;
	float reflection_mask = step(line_sharpness, combined);
	
	// 4. Gradient (We use UV.y here because the fade should stay relative to the sprite height)
	float depth_factor = clamp((UV.y - top_fade) / (bottom_fade - top_fade), 0.0, 1.0);
	float reflection_fade = 1.0 - depth_factor;
	
	vec4 tex_color = texture(TEXTURE, UV);
	vec3 ambient_bg = mix(tex_color.rgb, deep_water_color.rgb, depth_factor * bg_darkness);
	
	vec3 final_rgb = mix(ambient_bg, line_color.rgb, reflection_mask * line_color.a * reflection_fade);
	
	COLOR = vec4(final_rgb, tex_color.a);
}