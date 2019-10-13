<?php

namespace App\Repositories;

use App\Model\Post;

class PostRepository
{
    protected $post;
    
    public function __construct(Post $post)
	{
	    $this->post = $post;
    }

    public function get($where = null)
    {
        return $this->post->with('category')->get();
    }

    public function find($id)
    {
        //logger('find');

        return $this->post->with('category')->find($id);
    }
}