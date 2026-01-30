shader_type canvas_item;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_nearest;
uniform vec4 blue_tint : source_color = vec4(0.0, 0.4, 0.6, 0.3);

// Controls how large the light center is
uniform float light_radius = 0.5;
// Controls how "soft" the edge of the light is
uniform float light_softness = 0.6;
// How much to reduce the tint in the center (0.0 = no tint, 1.0 = full tint)
uniform float center_brightness = 0.2;

void fragment() {
    vec4 color = texture(SCREEN_TEXTURE, SCREEN_UV);
    
    // Calculate distance from center (0.5, 0.5)
    float dist = distance(SCREEN_UV, vec2(0.5, 0.5));
    
    // Create a smooth mask that is 0.0 at center and 1.0 at edges
    float mask = smoothstep(light_radius - light_softness, light_radius, dist);
    
    // Adjust the mask so the center isn't completely clear (unless you want it to be)
    float final_mask = mix(center_brightness, 1.0, mask);
    
    // Apply the tint, multiplied by our radial mask
    COLOR = mix(color, blue_tint, blue_tint.a * final_mask);
}