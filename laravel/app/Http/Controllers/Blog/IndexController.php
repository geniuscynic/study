<?php

namespace App\Http\Controllers\Blog;

use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Repositories\PostRepository;

class IndexController extends Controller
{
    protected $postRepository;
   
    public function __construct(PostRepository $post)
    {
        $this->postRepository = $post;
    } 
    //
    public function index() {
        return view('blog.index', ['posts'=> $this->postRepository->get()]);
    }

    public function archives($id)
    {
        # code...
        logger('archives');

        return view('blog.archive', ['post' => $this->postRepository->find($id)]);
    }
}
