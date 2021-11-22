<?php
add_filter(
    'register_post_type_args',
    function ($args, $post_type) {
        if ($post_type !== 'post') {
            return $args;
        }

        $args['rewrite'] = [
            'slug' => 'news',
            'with_front' => true,
        ];

        return $args;
    },
    10,
    2
);
add_filter(
    'pre_post_link',
    function ($permalink, $post) {
        if ($post->post_type !== 'post') {
            return $permalink;
        }

        return '/news/%postname%/';
    },
    10,
    2
);