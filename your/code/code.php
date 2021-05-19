<?php
function new_excerpt_more($more) {
	global $post;
    return '<a class="readmore" href="'. get_permalink($post->ID) . '">Read More</a>';
}
add_filter('excerpt_more', 'new_excerpt_more', 21 );