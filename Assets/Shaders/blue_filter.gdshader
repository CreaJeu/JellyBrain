shader_type canvas_item;

// filter_nearest keeps your pixels sharp
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_nearest;

// Adjust the 'a' (Alpha) in the inspector to control how "blue" it is
uniform vec4 blue_tint : source_color = vec4(0.0, 0.4, 0.6, 0.3);

void fragment() {
    // Grab the screen color with zero distortion
    vec4 color = texture(SCREEN_TEXTURE, SCREEN_UV);
    
    // Apply the tint
    COLOR = mix(color, blue_tint, blue_tint.a);
}