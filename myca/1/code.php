<?php
function my_acf_init() {	
    $mapapikey = get_field("google_map_api_key",'options');
	acf_update_setting('google_api_key', $mapapikey);
}
add_action('acf/init', 'my_acf_init');


/*remove woocommerce via hook*/
remove_action( 'woocommerce_before_main_content','woocommerce_breadcrumb', 20, 0);
remove_action( 'woocommerce_single_product_summary','woocommerce_template_single_title', 5);
remove_action( 'woocommerce_single_product_summary','woocommerce_template_single_rating', 10);
remove_action( 'woocommerce_single_product_summary','woocommerce_template_single_price', 10);
remove_action( 'woocommerce_single_product_summary','woocommerce_template_single_excerpt', 20);
remove_action( 'woocommerce_single_product_summary','woocommerce_template_single_meta', 40);
remove_action( 'woocommerce_single_product_summary','woocommerce_template_single_sharing', 50);

remove_action( 'woocommerce_after_single_product_summary','woocommerce_upsell_display', 15);
remove_action( 'woocommerce_after_single_product_summary','woocommerce_output_related_products', 20);

add_filter( 'woocommerce_product_tabs', 'woo_remove_product_tabs', 100 );
function woo_remove_product_tabs( $tabs ) {
    unset( $tabs['description'] );          
    unset( $tabs['additional_information'] );
    unset( $tabs['wcfm_product_multivendor_tab'] );
    unset( $tabs['wcfm_policies_tab']);
    unset( $tabs['wcfm_enquiry_tab']);
    
    return $tabs;
}
/*remove woocommerce via hook*/