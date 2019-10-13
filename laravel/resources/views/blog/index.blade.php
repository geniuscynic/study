@extends('blog._layout')

@section('title', 'xjjxmm')

@section('content')

@foreach ($posts as $post)
<article class="card mt-3">
    <h3 class="card-header entry-header">
        <a class="title-link" itemprop="url" title="{{$post['title']}}" href="{{URL::route('blog.archives', $post['id'])}}">
            {{$post['title']}}
        </a>
    </h3>
    <div class="card-body entry-content d-flex d-row">
        <img class="img-thumbnail col-md-4" src="" alt="暂无图片" />


        <div class="ml-3">
            {{$post['content']}}

        </div>
    </div>
    <footer class="card-footer navbar">
        <ul class="nav mr-auto">
            <li class="nav-item">时间： <time datetime="{{$post['updated_at']}}"
                    itemprop="datePublished">{{ date('Y-m-d' , strtotime($post['updated_at']) ) }}</time></li>
            <li class="nav-item">作者： <a itemprop="name"
                    href="{{URL::route('blog.archives', $post['id'])}}">{{$post['author']}}</a>
            </li>
            <li class="nav-item">分类： 
                <a href="{{URL::route('blog.archives', $post['id'])}}">
                    {{ $post->category['description'] }}
                </a>
            </li>
            <li class="nav-item">评论： 
                <a itemprop="discussionUrl"
                    href="{{URL::route('blog.archives', $post['id'])}}">0
                    条</a></li>

        </ul>
        <a class="nav-item" href="{{URL::route('blog.archives', $post['id'])}}">阅读全文</a>
    </footer>
</article>
@endforeach

<nav aria-label="Page navigation" class="mt-3">
                    <ul class="pagination">
                        <li class="page-item">
                            <a class="page-link" href="#" aria-label="Previous">
                                <span aria-hidden="true">&laquo;</span>
                            </a>
                        </li>
                        <li class="page-item"><a class="page-link" href="#">1</a></li>
                        <li class="page-item"><a class="page-link" href="#">2</a></li>
                        <li class="page-item"><a class="page-link" href="#">3</a></li>
                        <li class="page-item">
                            <a class="page-link" href="#" aria-label="Next">
                                <span aria-hidden="true">&raquo;</span>
                            </a>
                        </li>
                    </ul>
                </nav>
<!-- <article class="card mt-3">
    <h3 class="card-header entry-header"><a class="title-link" itemprop="url" title="【声明】本主题不再维护，联系方式见文章内"
            href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/archives/43/"><span style="color:red">[置顶]
            </span>【声明】本主题不再维护，联系方式见文章内</a></h3>
    <div class="card-body entry-content d-flex d-row">
        <img class="img-thumbnail col-md-4" src="" alt="暂无图片" />


        <div class="ml-3">
            声明因为时间问题，本主题已经不再维护和升级，有问题请到 http://contact.ptbird.cn 联系我。谢谢。
            声明因为时间问题，本主题已经不再维护和升级，有问题请到 http://contact.ptbird.cn 联系我。谢谢。
            声明因为时间问题，本主题已经不再维护和升级，有问题请到 http://contact.ptbird.cn 联系我。谢谢。
            声明因为时间问题，本主题已经不再维护和升级，有问题请到 http://contact.ptbird.cn 联系我。谢谢。
            声明因为时间问题，本主题已经不再维护和升级，有问题请到 http://contact.ptbird.cn 联系我。谢谢。

        </div>
    </div>
    <footer class="card-footer navbar">
        <ul class="nav mr-auto">
            <li class="nav-item">时间： <time datetime="2018-12-03T23:35:00+08:00"
                    itemprop="datePublished">2018-12-03</time></li>
            <li class="nav-item">作者： <a itemprop="name"
                    href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/author/1/">admin</a>
            </li>
            <li class="nav-item">分类： <a
                    href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/category/default/">默认分类</a>
            </li>
            <li class="nav-item"><a itemprop="discussionUrl"
                    href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/archives/43/#comments">0
                    条评论</a></li>

        </ul>
        <a class="nav-item" href="http://theme.typecho.ptbird.cn/bootstrap-blog/index.php/archives/43/">阅读全文</a>
    </footer>
</article> -->
@endsection
