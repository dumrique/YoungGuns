var cleanCss = require('gulp-clean-css');
var clean = require('gulp-clean');
var connect = require('gulp-connect');
var concat = require('gulp-concat');
var gulp = require('gulp');
var jasmine = require('gulp-jasmine');
var jasmineSpecReporter = require('jasmine-spec-reporter');
var runSequence = require('run-sequence');
var sass = require('gulp-sass');
var uglify = require('gulp-uglify');
var webpack = require('webpack-stream');

const buildPath = './dist';

gulp.task('clean', function() {
    return gulp.src(buildPath, { read: false })
        .pipe(clean());
});

gulp.task('build:js', function () {
    return gulp.src('./src/app/youngguns.module.js')
      .pipe(webpack({
          output: {
              filename: 'app.min.js'
          }
      }))
      .pipe(uglify())
      .pipe(gulp.dest(buildPath));
});

gulp.task('build:html', function () {
    return gulp.src('./src/**/*.html')
      .pipe(gulp.dest(buildPath));
});

gulp.task('build:css', function () {
    return gulp.src('./src/css/youngguns.scss')
        .pipe(sass())
        .pipe(cleanCss())
        .pipe(concat('styles.css'))
        .pipe(gulp.dest(buildPath));
});

gulp.task('test', function() {
    return gulp.src('src/app/*/*.spec.js')
        .pipe(jasmine({ reporter: new jasmineSpecReporter() }));
});

gulp.task('build', function () {
    runSequence(['build:js', 'build:html', 'build:css']);
});

gulp.task('host', function () {
    connect.server({
        root: 'dist',
        port: 5000,
        livereload: true
    });
});

gulp.task('watch', function () {
    gulp.watch('./src/app/**/*.html', ['build:html']);
    gulp.watch('./src/*.html', ['build:html']);
    gulp.watch('./src/app/**/*.js', ['build:js']);
    gulp.watch('./src/app/*.js', ['build:js']);
    gulp.watch('./src/css/**/*.scss', ['build:css']);
    gulp.watch('./src/css/**/*.css', ['build:css']);
});

gulp.task('default', function () {
    runSequence(['build', 'host'],
                'watch');
});