<?php
add_action('rest_api_init', 'add_rest_api_toget_data');
function add_rest_api_toget_data()
{

    register_rest_route(

        'getorderdata/v1', '/getzohoid/', [

            'methods' => 'POST',

            'callback' => 'get_user_tokendata',

            'args' => [

                'order_id' => [

                    'required' => true,

                    'type' => 'string',

                    'description' => 'The order id',

                    //'format' => 'email'

                ],
                'zoho_id' => [

                    'required' => true,

                    'type' => 'string',

                    'description' => 'The zoho id',

                    //'format' => 'email'

                ],

            ],

        ]

    );

}
function get_user_tokendata($data)
{

    global $wpdb;

    $orderid = $data->get_param("order_id");
    $zohoid = $data->get_param("zoho_id");
    $dataoftoken = ['success' => 'false'];
    if (!empty($orderid) && !empty($zohoid)) {
        update_post_meta($orderid, 'zoho_invoice_id', $zohoid);
        $dataoftoken = ['success' => 'true'];
    }
    echo json_encode($dataoftoken);
    exit;
}