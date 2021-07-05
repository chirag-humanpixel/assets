<?php
add_action( 'init', function () {
	if ( function_exists( 'add_ux_builder_post_type' ) ) {
		add_ux_builder_post_type( 'our_labs' );
        add_ux_builder_post_type( 'our_casestudy' );
        add_ux_builder_post_type( 'our_event' );
        add_ux_builder_post_type( 'our_job' );
        add_ux_builder_post_type( 'our_publication' );
        add_ux_builder_post_type( 'our_ma' );
        add_ux_builder_post_type( 'our_award' );
	}
});