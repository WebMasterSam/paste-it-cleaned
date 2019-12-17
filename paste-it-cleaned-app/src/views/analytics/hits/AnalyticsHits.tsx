import React from 'react'

import { Paper, Typography } from '@material-ui/core'
import FileCopyIcon from '@material-ui/icons/FileCopy'

import FormWrapper from '../../../components/FormWrapper'
import { createData } from '../../../helpers/AnalyticsHelper'

import './AnalyticsHits.less'
import HitsTable from './components/HitsTable'

export interface AnalyticsHitsProps {}
export interface AnalyticsHitsState {}

const rows = [
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Word', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '1.1.2.3', 'Excel', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'PowerPoint', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'Web', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'Text', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
    createData(new Date(2019, 5, 26, 11, 12, 0), '123.144.255.366', 'Image', 'useragent. micosoft, android, blah bla// window / 8.0', 0.05),
]

class AnalyticsHits extends React.Component<AnalyticsHitsProps, AnalyticsHitsState> {
    render() {
        return (
            <Paper className="paper wide">
                <Typography variant="h2" className="override-h2" component="h2">
                    <FileCopyIcon /> Last plugin uses
                </Typography>

                <FormWrapper>
                    <HitsTable rows={rows} full={true} />
                </FormWrapper>
            </Paper>
        )
    }
}

export default AnalyticsHits
