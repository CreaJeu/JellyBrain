shader_type canvas_item;

uniform vec4 line_color : source_color = vec4(1.0, 1.0, 1.0, 0.2);
uniform vec4 deep_water_color : source_color = vec4(0.0, 0.05, 0.1, 1.0);
uniform float pixel_size : hint_range(1.0, 10.0) = 1.0;
uniform float wave_speed : hint_range(0.0, 5.0) = 0.4;
uniform float base_scale : hint_range(1.0, 1000.0) = 150.0;
uniform float line_sharpness : hint_range(0.0, 1.0) = 0.96;
uniform float warp_strength : hint_range(0.0, 5.0) = 0.8; // Controls the "chaos" wiggle

uniform float top_fade : hint_range(-1.0, 1.0) = -0.2;   
uniform float bottom_fade : hint_range(0.0, 2.0) = 1.2; 
uniform float bg_darkness : hint_range(0.0, 1.0) = 0.6;
uniform vec2 scroll_offset = vec2(0.0);

void fragment() {
    // 1. World Coordinates
    vec2 world_uv = SCREEN_UV + (scroll_offset * 0.001); 
    vec2 screen_res = 1.0 / SCREEN_PIXEL_SIZE;
    vec2 grid_uv = round(world_uv * (screen_res / pixel_size)) / (screen_res / pixel_size);
    float t = round(TIME * 8.0) / 8.0;

    // 2. DOMAIN WARPING (The Chaos Generator)
    // We create a "distortion field" using sine/cosine combinations
    float warp = sin(grid_uv.x * 12.0 + t) * cos(grid_uv.y * 15.0 + t * 0.5);
    vec2 warped_uv = grid_uv + (warp * 0.01 * warp_strength);

    // 3. Multiplicative Waves (Interference patterns)
    // By using different multipliers and the warped UV, we get irregular shapes
    float wave1 = sin((warped_uv.x * 1.4 + warped_uv.y * 0.9 + t * wave_speed) * base_scale);
    float wave2 = sin((warped_uv.x * 0.8 - warped_uv.y * 1.6 + t * (wave_speed * 0.6)) * (base_scale * 1.3));
    float wave3 = cos((warped_uv.x * 1.1 + warped_uv.y * 0.4 + t * (wave_speed * 0.3)) * (base_scale * 0.7));
    
    // Combining by multiplying and adding different phases creates a "noise-like" look
    // without actually needing a noise texture.
    float combined = (wave1 * wave2) + (wave3 * 0.4);
    
    // 4. Threshold & Jitter
    // We add a tiny bit of time-based flicker to make the sparkles "dance"
    float flicker = sin(t * 10.0) * 0.02;
    float reflection_mask = step(line_sharpness + flicker, combined);
    
    // 5. Gradient & Depth
    float depth_factor = clamp((UV.y - top_fade) / (bottom_fade - top_fade), 0.0, 1.0);
    float reflection_fade = 1.0 - depth_factor;
    
    vec4 tex_color = texture(TEXTURE, UV);
    vec3 ambient_bg = mix(tex_color.rgb, deep_water_color.rgb, depth_factor * bg_darkness);
    
    vec3 final_rgb = mix(ambient_bg, line_color.rgb, reflection_mask * line_color.a * reflection_fade);
    
    COLOR = vec4(final_rgb, tex_color.a);
}