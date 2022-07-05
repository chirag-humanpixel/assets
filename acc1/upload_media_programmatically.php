<?php

// Create HTML Form
?>
    <form action="<?php echo get_stylesheet_directory_uri() ?>/process_upload.php" method="post" enctype="multipart/form-data">
        Your Photo: <input type="file" name="profilepicture" size="25" />
        <input type="submit" name="submit" value="Submit" />
    </form>
<?php

// Process the Uploaded File in PHP and Add the File Metadata to WordPress Database

require( ABSPATH . 'wp-load.php' );
$wordpress_upload_dir = wp_upload_dir();

$profilepicture = $_FILES['profilepicture'];

$new_file_path = $wordpress_upload_dir['path'] . '/' . $profilepicture['name'];
$new_file_mime = mime_content_type( $profilepicture['tmp_name'] );

if( empty( $profilepicture ) ){
    die( 'File is not selected.' );
}
if( $profilepicture['error'] )
	die( $profilepicture['error'] );
	
if( $profilepicture['size'] > wp_max_upload_size() )
	die( 'It is too large than expected.' );
	
if( !in_array( $new_file_mime, get_allowed_mime_types() ) )
	die( 'WordPress doesn\'t allow this type of uploads.' );

while( file_exists( $new_file_path ) ) {
    $centerpath++;
    $new_file_path = $wordpress_upload_dir['path'] . '/' . $centerpath . '_' . $profilepicture['name'];
}

if( move_uploaded_file( $profilepicture['tmp_name'], $new_file_path ) ) {

    $upload_id = wp_insert_attachment( array(
        'guid'           => $new_file_path, 
        'post_mime_type' => $new_file_mime,
        'post_title'     => preg_replace( '/\.[^.]+$/', '', $profilepicture['name'] ),
        'post_content'   => '',
        'post_status'    => 'inherit'
    ), $new_file_path );

    require_once( ABSPATH . 'wp-admin/includes/image.php' );

    wp_update_attachment_metadata( $upload_id, wp_generate_attachment_metadata( $upload_id, $new_file_path ) );
}