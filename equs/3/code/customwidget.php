<?php
class jobcat_list_widget extends WP_Widget 

{

    function __construct() 

    {

        parent::__construct(

            'jobcat_list_widget',

            __('Job Category Widget', ' '),

            array( 'description' => __( 'Job Category Widget', '' ))

        );

    }

    public function widget( $args, $instance ) 

    {

        $title = apply_filters( 'widget_title', $instance['title'] );

        $showcategory = explode( ',',$instance['showcategory']);

        echo $args['before_widget'];

        if ( ! empty( $title ) )

        {

            echo $args['before_title'] . $title . $args['after_title'];

        }

        $terms = get_terms( array(

            'taxonomy' => 'job_cat',

            'include' => $showcategory,

        ) );

            if($terms)

            {

                echo '<ul>';

                foreach($terms as $term)

                {

                $term_link = get_term_link( $term );

        ?>

                <li class="cat-item cat-item-<?php echo $term->term_id; ?>"><a href="<?php echo $term_link; ?>"><?php echo $term->name; ?></a></li>

        <?php

                }

                echo '</ul>';

            }

            echo $args['after_widget'];

    }

    public function form( $instance ) 

    {

        if ( isset( $instance[ 'title' ] ) )

        {$title = $instance[ 'title' ];}

        else

        {

            $title = __( 'Default Title', 'hstngr_widget_domain' );

        }

        if ( isset( $instance[ 'showcategory' ] ) )

        { 

            $showcategory = explode(',',$instance[ 'showcategory' ]);

        }

        else

        {

            $showcategory = [];

        }

        

    ?>

        <p>

            <label for="<?php echo $this->get_field_id( 'title' ); ?>"><?php _e( 'Title:' ); ?></label>

            <input class="widefat" id="<?php echo $this->get_field_id( 'title' ); ?>" name="<?php echo $this->get_field_name( 'title' ); ?>" type="text" value="<?php echo esc_attr( $title ); ?>" />

        </p>

        <p>

            <label for="<?php echo $this->get_field_id( 'showcategory' ); ?>"><?php _e( 'Select category:' ); ?></label>

            <select name="<?php echo $this->get_field_name( 'showcategory' ); ?>[]" id="<?php echo $this->get_field_id( 'showcategory' ); ?>" multiple="multiple">

            <?php

                 $categories = get_terms( array(

                    'taxonomy' => 'job_cat',

                ));

                    

                foreach( $categories as $category ) 

                {

                    $selected = '';

                    if(in_array($category->term_id,$showcategory))

                    {

                        $selected = 'selected';

                    }

                    ?>

                        <option value="<?php echo $category->term_id; ?>" <?php echo $selected; ?>><?php echo $category->name; ?></option>

                    <?php

                } 

            ?>

            </select>



        </p>

    <?php

    }

    public function update( $new_instance, $old_instance ) 

    {

        

        $instance = array();

        $instance['title'] = ( ! empty( $new_instance['title'] ) ) ? strip_tags( $new_instance['title'] ) : '';

        $instance['showcategory'] = ( ! empty( $new_instance['showcategory'] ) ) ? implode( ',', $new_instance['showcategory'] ) : '';

        return $instance;

    }

}
