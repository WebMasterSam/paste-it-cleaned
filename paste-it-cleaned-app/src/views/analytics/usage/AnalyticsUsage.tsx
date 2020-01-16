import React, { Fragment } from 'react'
import moment from 'moment'
import { CartesianGrid, XAxis, YAxis, Legend, Tooltip, LineChart, Line, ResponsiveContainer } from 'recharts'

import { Paper, Typography, Button } from '@material-ui/core'
import FileCopyIcon from '@material-ui/icons/FileCopy'
import SkipPreviousIcon from '@material-ui/icons/SkipPrevious'
import SkipNextIcon from '@material-ui/icons/SkipNext'

import { WordIcon } from '../../../icons/Word'
import { ExcelIcon } from '../../../icons/Excel'
import { PowerPointIcon } from '../../../icons/PowerPoint'

import { WebIcon } from '../../../icons/Web'
import { TextIcon } from '../../../icons/Text'
import { ImageIcon } from '../../../icons/Image'

import { HitDaily } from '../../../entities/api'
import { AnalyticsUsageController } from './AnalyticsUsageController'
import ButtonWithLoading from '../../../components/ButtonWithLoading'

import './AnalyticsUsage.less'
import { Skeleton } from '@material-ui/lab'

export interface AnalyticsUsageProps {}
export interface AnalyticsUsageState {
    startDate: Date
    endDate: Date
    hitsDaily: HitDaily[]
    hitsDailyLoading: boolean
    hitsDailyError: boolean
    officeUsage: any[]
    websiteUsage: any[]
    localUsage: any[]
}

class AnalyticsUsage extends React.Component<AnalyticsUsageProps, AnalyticsUsageState> {
    private controller?: AnalyticsUsageController = undefined

    constructor(props: AnalyticsUsageProps) {
        super(props)
        this.controller = new AnalyticsUsageController(this)
        this.state = {
            hitsDaily: [],
            hitsDailyLoading: false,
            hitsDailyError: false,
            startDate: moment()
                .startOf('month')
                .toDate(),
            endDate: moment()
                .endOf('month')
                .toDate(),
            officeUsage: [],
            websiteUsage: [],
            localUsage: [],
        }
    }

    componentDidMount() {
        this.controller!.initialize()
    }

    render() {
        return (
            <Fragment>
                {this.getFilters()}
                {this.getOfficeChart()}
                {this.getWebsiteChart()}
                {this.getLocalChart()}
            </Fragment>
        )
    }

    private getFilters() {
        return (
            <Fragment>
                <div className="filters">
                    {this.state.startDate >
                        moment()
                            .subtract(5, 'years')
                            .toDate() && (
                        <ButtonWithLoading loading={this.state.hitsDailyLoading} variant="outlined" startIcon={<SkipPreviousIcon />} onClick={this.controller!.previous}>
                            Prev
                        </ButtonWithLoading>
                    )}
                    <Typography variant="body1" className="filter-month-label" component="span">
                        {moment(this.state.startDate).format('MMMM YYYY')}
                    </Typography>
                    {this.state.endDate < new Date() && (
                        <ButtonWithLoading loading={this.state.hitsDailyLoading} variant="outlined" endIcon={<SkipNextIcon />} onClick={this.controller!.next}>
                            Next
                        </ButtonWithLoading>
                    )}
                </div>
            </Fragment>
        )
    }

    private getOfficeChart() {
        return (
            <Paper className="paper wide">
                <Typography variant="h2" className="override-h2" component="h2">
                    <FileCopyIcon /> Office plugin usage
                </Typography>
                {this.getMonthSubtitle()}
                <ResponsiveContainer width="100%" height={400}>
                    <LineChart data={this.state.officeUsage} margin={{ top: 30, right: 50, left: 0, bottom: 10 }}>
                        {this.getCartesianGrid()}
                        {this.getXAxis()}
                        {this.getYAxis()}
                        {this.getCustomToolTip()}
                        {this.getLegend()}
                        <Line type="monotone" dataKey="Word" stroke="#1565C0" legendType="circle" activeDot={<WordDot />} />
                        <Line type="monotone" dataKey="Excel" stroke="#388E3C" legendType="circle" activeDot={<ExcelDot />} />
                        <Line type="monotone" dataKey="PowerPoint" stroke="#FF5722" legendType="circle" activeDot={<PowerPointDot />} />
                    </LineChart>
                </ResponsiveContainer>
            </Paper>
        )
    }

    private getWebsiteChart() {
        return (
            <Paper className="paper wide">
                <Typography variant="h2" className="override-h2" component="h2">
                    <FileCopyIcon /> Website plugin usage
                </Typography>
                {this.getMonthSubtitle()}
                <ResponsiveContainer width="100%" height={400}>
                    <LineChart data={this.state.websiteUsage} margin={{ top: 30, right: 50, left: 0, bottom: 10 }}>
                        {this.getCartesianGrid()}
                        {this.getXAxis()}
                        {this.getYAxis()}
                        {this.getCustomToolTip()}
                        {this.getLegend()}
                        <Line type="monotone" dataKey="Web" stroke="#d92b09" legendType="circle" activeDot={<WebDot />} />
                    </LineChart>
                </ResponsiveContainer>
            </Paper>
        )
    }

    private getLocalChart() {
        return (
            <Paper className="paper wide">
                <Typography variant="h2" className="override-h2" component="h2">
                    <FileCopyIcon /> Local plugin usage
                </Typography>
                {this.getMonthSubtitle()}
                <ResponsiveContainer width="100%" height={400}>
                    <LineChart data={this.state.localUsage} margin={{ top: 30, right: 50, left: 0, bottom: 10 }}>
                        {this.getCartesianGrid()}
                        {this.getXAxis()}
                        {this.getYAxis()}
                        {this.getCustomToolTip()}
                        {this.getLegend()}
                        <Line type="monotone" dataKey="Text" stroke="#333" legendType="circle" activeDot={<TextDot />} />
                        <Line type="monotone" dataKey="Image" stroke="#18a4f0" legendType="circle" activeDot={<ImageDot />} />
                    </LineChart>
                </ResponsiveContainer>
            </Paper>
        )
    }

    private getMonthSubtitle() {
        return (
            <Typography variant="subtitle1" component="p" align="center">
                {moment(this.state.startDate).format('MMMM YYYY')}
            </Typography>
        )
    }

    private getLegend() {
        return <Legend />
    }

    private getCartesianGrid() {
        return <CartesianGrid strokeDasharray="3 3" />
    }

    private getXAxis() {
        return <XAxis dataKey="name" />
    }

    private getYAxis() {
        return <YAxis label={{ value: 'Pastes', angle: -90, position: 'insideLeft' }} />
    }

    private getCustomToolTip() {
        return (
            <Tooltip
                offset={30}
                contentStyle={{ borderRadius: 10 }}
                formatter={(value, name, props) => {
                    return value + ' paste' + ((value as number) > 1 ? 's' : '')
                }}
                itemStyle={{ lineHeight: 0.6 }}
                labelStyle={{ marginBottom: 10, fontWeight: 'bold' }}
                labelFormatter={(label: string | number) => {
                    var theDate = new Date(this.state.startDate.getFullYear(), this.state.startDate.getMonth(), label as number)
                    return theDate.toLocaleDateString()
                }}
            />
        )
    }
}

const WordDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <WordIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

const ExcelDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <ExcelIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

const PowerPointDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <PowerPointIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

const WebDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <WebIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

const TextDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <TextIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

const ImageDot = (props: any) => {
    const radius = 12
    const diameter = radius * 4
    return (
        <svg width={diameter} height={diameter} style={{ overflow: 'visible' }}>
            <circle cx={props.cx} cy={props.cy} r={radius} fill="#fff" stroke="#ccc" />
            <ImageIcon height={15} width={15} x={props.cx - 7.5} y={props.cy - 7.5} />
        </svg>
    )
}

export default AnalyticsUsage
