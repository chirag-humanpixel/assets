<?php
add_action('woocommerce_add_order_item_meta', 'wdm_add_values_to_order_item_meta', 1, 2);
if (!function_exists('wdm_add_values_to_order_item_meta')) {
    function wdm_add_values_to_order_item_meta($item_id, $values)
    {
        global $woocommerce, $wpdb;
        $camp_date = $values['camp_date'];
        $radiogroup = $values['radiogroup'];
        if (!empty($camp_date) && !empty($radiogroup)) {
            wc_add_order_item_meta($item_id, 'Camp Name', $camp_date);
            wc_add_order_item_meta($item_id, 'Pick Up Option', $radiogroup);
        }
    }
}
add_filter('woocommerce_get_cart_item_from_session', 'wdm_get_cart_items_from_session', 1, 3);
if (!function_exists('wdm_get_cart_items_from_session')) {
    function wdm_get_cart_items_from_session($item, $values, $key)
    {
        if (array_key_exists('camp_date', $values) && array_key_exists('radiogroup', $values)) {
            $item['camp_date'] = $values['camp_date'];
            $item['radiogroup'] = $values['radiogroup'];
        }
        return $item;
    }
}
add_filter('woocommerce_add_cart_item_data', 'wdm_add_item_data', 1, 2);
if (!function_exists('wdm_add_item_data')) {
    function wdm_add_item_data($cart_item_data, $product_id)
    {

        @session_start();
        /*Here, We are adding item in WooCommerce session with, wdm_user_custom_data_value name*/
        global $woocommerce;
        $camp_date = $radiogroup = '-';
        if (isset($_POST['camp_date']) && isset($_POST['radiogroup'])) {
            $camp_date = $_POST['camp_date'];
            $radiogroup = $_POST['radiogroup'];
        }
        $new_value = array('camp_date' => $camp_date, 'radiogroup' => $radiogroup);
        if (empty($camp_date) && empty($radiogroup)) {
            return $cart_item_data;
        } else {
            if (empty($cart_item_data)) {
                return $new_value;
            } else {
                return array_merge($cart_item_data, $new_value);
                $_SESSION['camp_date'] = $new_value['camp_date'];
                $_SESSION['radiogroup'] = $new_value['radiogroup'];
            }
        }
    }
}
add_action('woocommerce_checkout_create_order_line_item', 'save_order_item_product_fitting_color', 10, 4);
function save_order_item_product_fitting_color($item, $cart_item_key, $values, $order)
{
    if (isset($values['camp_date']['value'])) {
        $key = __('Camp Name', 'woocommerce');
        $value = $values['camp_date']['value'];
        $item->update_meta_data($key, $value);
    }
    if (isset($values['radiogroup']['value'])) {
        $key = __('Pick Up Option', 'woocommerce');
        $value = $values['radiogroup']['value'];
        $item->update_meta_data($key, $value);
    }
}