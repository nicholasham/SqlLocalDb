'use strict';

var gulp = require('gulp'),
    del = require('del'),
    fs = require('fs'),
    glob = require('glob'),
    util = require('gulp-util'),
    clean = require('gulp-clean'),
    path = require('path'),
    msbuild = require('gulp-msbuild'),
    xunit = require('gulp-xunit-runner'),
    Nuget = require('nuget-runner'),
    assemblyInfo = require('gulp-dotnet-assembly-info');


var paths = {};
paths.root = __dirname;
paths.artifactsDirectory = path.join(paths.root, 'artifacts');
paths.sourceDirectory = path.join(paths.root, 'src');
paths.solutionDirectory = paths.sourceDirectory;
paths.solutionFilePattern = path.join(paths.solutionDirectory, '*.sln');
paths.nugetDirectory = path.join(paths.solutionDirectory, '.nuget');
paths.nugetPackagesDirectory = path.join(paths.solutionDirectory, 'packages');
paths.nugetNuspecPattern = path.join(paths.solutionDirectory, '**/*.nuspec');
paths.nugetExeFile = path.join(paths.nugetDirectory, 'NuGet.exe');
paths.nugetConfigFile = path.join(paths.nugetDirectory, 'NuGet.config');
paths.projectLicenceUrl = '';


var buildinfo = {};
buildinfo.configuration = 'Release';
buildinfo.number = process.env.BUILD_NUMBER || 0;
buildinfo.packageVersion = '1.1.0'
buildinfo.version = buildinfo.packageVersion + '.' + buildinfo.number;

var nuget = new Nuget({
    nugetPath: paths.nugetExeFile,
    apiKey: '',
    verbosity: 'normal',
    configFile: paths.nugetConfigFile
});


// Return a stream so gulp can determine completion
gulp.task('clean', function (done) {

    del([paths.artifactsDirectory, paths.nugetPackagesDirectory], function (err, paths) {
        console.log('Deleted files/folders:\n', path.join('\n'));
        done();
    });

});

gulp.task('nuget-restore', ['clean'], function () {

    nuget
        .restore({
            packages: paths.solutionDirectory
        });
});

gulp.task('configuration', function () {

});


gulp.task('package', ['nuget-pack', 'assemblyInfo'], function () {});

gulp.task('nuget-pack', ['nuget-restore', 'build', 'test'], function () {


    var nuspecProperties = {
        authors: 'Nicholas Hammond',
        owners: 'Nicholas Hammond',
        licenseUrl: 'https://github.com/nicholasham/SqlLocalDb/license.txt',
        projectUrl: 'https://github.com/nicholasham/SqlLocalDb',
        iconUrl: 'http://github.com',
        requireLicenseAcceptance: false,
        releaseNotes: '',
        copyright: 'Copyright (2015)',
        configuration: buildinfo.configuration
    };


    fs.mkdirSync(paths.artifactsDirectory);

    var options = {
        nonull: false
    };

    glob(paths.nugetNuspecPattern, options, function (error, nuspecFiles) {


        if (nuspecFiles.length === 0) {
            util.log('NuGet: No nuspec files found in projects to pack');
            return;
        }

        for (var index = 0; index < nuspecFiles.length; index++) {

            var nuspecFile = nuspecFiles[index];
            var projectFile = nuspecFile.replace('.nuspec', '.csproj');

            nuget.pack({
                spec: projectFile,
                outputDirectory: paths.artifactsDirectory,
                version: buildinfo.packageVersion,
                symbols: true,
                includeReferencedProjects: true,
                properties: nuspecProperties
            });
        }
    })


});

gulp.task('xunit-test', ['build'], function () {

    var files = glob.sync('**/Xunit.console.exe');

    if (files.length === 0) {
        throw new Error('XUnit console is not installed, Please install package xunit.runner.console using nuget');
    }

    var unitTestsPath = path.join(path.join('**/bin', buildinfo.configuration), '*UnitTests*.dll');

    return gulp.src([unitTestsPath], {
            read: false
        })
        .pipe(xunit({
            executable: files[0]
        }));
});

gulp.task('test');

gulp.task('assemblyInfo', function () {


    var replacements = {
        configuration: buildinfo.configuration,
        copyright: 'Copyright 2015',
        version: buildinfo.version,
        fileVersion: buildinfo.version,
    };

    gulp.src('**/AssemblyInfo.cs')
        .pipe(assemblyInfo(replacements))
        .pipe(gulp.dest('.'));
});

gulp.task('build', ['assemblyInfo', 'nuget-restore'], function () {

    var options = {
        configuration: buildinfo.configuration,
        toolsVersion: 12.0,
        targets: ['Clean', 'Build'],
        errorOnFail: true,
        stdout: true,
        verbosity: 'quiet',
        properties: {
            clp: 'ErrorsOnly'
        }
    };

    return gulp
        .src('**/*.sln')
        .pipe(msbuild(options));
});


gulp.task('default', ['clean', 'build', 'test', 'package']);

gulp.task('ci', ['clean', 'build', 'test', 'package']);