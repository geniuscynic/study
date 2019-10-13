@extends('blog._layout')

@section('title', 'xjjxmm')

@section('content')



<article class="card mt-3">
   
    <div class="card-body entry-content d-flex flex-column ">
        <h2 class="card-title mx-auto" > {{ $post['title'] }}</h2>
        <ul class="nav mx-auto">
            <li class="nav-item">时间： <time datetime="{{$post['updated_at']}}"
                    itemprop="datePublished">{{ date('Y-m-d' , strtotime($post['updated_at']) ) }}</time></li>
            <li class="nav-item">作者： <a itemprop="name"
                    href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/author/1/">{{ $post['author'] }}</a>
            </li>
            <li class="nav-item">分类： <a
                    href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/category/default/">{{ $post->category['description'] }}</a>
            </li>
            
        </ul>


        <div class="mt-3">
            {{ $post['content'] }}
        </div>
    </div>
    
</article>
@endsection
